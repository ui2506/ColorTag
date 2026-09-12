using LabApi.Features.Wrappers;
using LiteDB;
using MEC;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColorTag
{
    internal sealed class PlayerPrefix
    {
        [BsonId]
        public string UserId { get; set; }
        public List<string> Colors { get; set; }

        internal static ILiteCollection<PlayerPrefix> PlayerInfoCollection => Plugin.Data.GetCollection<PlayerPrefix>($"ColorSetting{Server.Port}");

        internal static async Task InsertPlayerAsync(Player player, List<string> colors)
        {
            PlayerPrefix insert = new PlayerPrefix()
            {
                UserId = player.UserId,
                Colors = new List<string>() { player.GroupColor }
            };

            _ = PlayerInfoCollection.Insert(insert);
        }

        internal static bool TryGetValue(string userId, out PlayerPrefix info)
        {
            info = PlayerInfoCollection.FindById(userId);
            return info != null;
        }

        internal static bool Contains(string userId) => PlayerInfoCollection.FindById(userId) != null;

        internal static void DeletePlayer(string userId)
        {
            if (Contains(userId))
                PlayerInfoCollection.Delete(userId);
        }

        internal static void DeleteAll() => PlayerInfoCollection.DeleteAll();

        internal static void GiveCoroutine(Player player)
        {
            if (player.UserGroup == null || player.GroupColor == null)
                return;

            if (!PlayerPrefix.TryGetValue(player.UserId, out PlayerPrefix info))
                return;

            if (Plugin.PlayerCoroutines.TryGetValue(player, out CoroutineHandle coroutine))
                Timing.KillCoroutines(coroutine);

            Plugin.PlayerCoroutines[player] = Timing.RunCoroutine(Coroutines.ChangeColor(player, info.Colors));
        }
    }
}
