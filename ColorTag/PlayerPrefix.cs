using LabApi.Features.Wrappers;
using LiteDB;
using MEC;
using System.Collections.Generic;

namespace ColorTag
{
    internal sealed class PlayerPrefix
    {
        [BsonId]
        public string UserId { get; set; }
        public List<string> Colors { get; set; }

        private static ILiteCollection<PlayerPrefix> PlayerInfoCollection;

        internal static void Initialize() => PlayerInfoCollection = Plugin.Data.GetCollection<PlayerPrefix>($"ColorSetting{Server.Port}");

        internal static void SetColors(Player player, IEnumerable<string> colors)
        {
            PlayerInfoCollection.Upsert(new PlayerPrefix
            {
                UserId = player.UserId,
                Colors = new List<string>(colors)
            });
        }

        internal static bool TryGetValue(string userId, out PlayerPrefix info)
        {
            info = PlayerInfoCollection.FindById(userId);
            return info != null;
        }

        internal static void Save(PlayerPrefix info)
        {
            if (info.Colors == null)
                info.Colors = new List<string>();

            PlayerInfoCollection.Upsert(info);
        }

        internal static bool DeletePlayer(string userId) => PlayerInfoCollection.Delete(userId);

        internal static void DeleteAll() => PlayerInfoCollection.DeleteAll();

        internal static void GiveCoroutine(Player player)
        {
            if (string.IsNullOrEmpty(player.UserId) || string.IsNullOrEmpty(player.GroupColor) || player.UserGroup == null)
                return;

            if (!TryGetValue(player.UserId, out PlayerPrefix info))
                return;

            StopCoroutine(player);

            Plugin.PlayerCoroutines[player] = Timing.RunCoroutine(Coroutines.ChangeColor(player, info.Colors));
        }

        internal static void StopCoroutine(Player player)
        {
            if (Plugin.PlayerCoroutines.TryGetValue(player, out CoroutineHandle coroutine))
                Timing.KillCoroutines(coroutine);
        }

        internal static void StopAllCoroutines()
        {
            foreach (CoroutineHandle coroutine in Plugin.PlayerCoroutines.Values)
                Timing.KillCoroutines(coroutine);

            Plugin.PlayerCoroutines.Clear();
        }
    }
}
