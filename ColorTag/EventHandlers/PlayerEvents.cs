using LabApi.Events.Arguments.Interfaces;
using LabApi.Events.Arguments.PlayerEvents;
using MEC;

namespace ColorTag.EventHandlers
{
    internal static class PlayerEvents
    {
        internal static void Register()
        {
            LabApi.Events.Handlers.PlayerEvents.Joined += TryGiveCoroutinve;
            LabApi.Events.Handlers.PlayerEvents.GroupChanged += TryGiveCoroutinve;
            LabApi.Events.Handlers.PlayerEvents.Left += OnLeft;
        }

        internal static void Unregister()
        {
            LabApi.Events.Handlers.PlayerEvents.Joined -= TryGiveCoroutinve;
            LabApi.Events.Handlers.PlayerEvents.GroupChanged -= TryGiveCoroutinve;
            LabApi.Events.Handlers.PlayerEvents.Left += OnLeft;
        }

        private static void OnLeft(PlayerLeftEventArgs ev)
        {
            if (ev.Player != null || !Plugin.PlayerCoroutines.TryGetValue(ev.Player, out CoroutineHandle coroutine))
                return;

            Timing.KillCoroutines(coroutine);
            Plugin.PlayerCoroutines.Remove(ev.Player);
        }

        private static void TryGiveCoroutinve<T>(T ev) where T : IPlayerEvent => PlayerPrefix.GiveCoroutine(ev.Player);
    }
}
