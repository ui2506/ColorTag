using LabApi.Events.Arguments.Interfaces;
using LabApi.Events.Arguments.PlayerEvents;

namespace ColorTag
{
    internal static class EventHandlers
    {
        private static bool _registered;

        internal static void Register()
        {
            if (_registered)
                return;

            LabApi.Events.Handlers.PlayerEvents.Joined += TryGiveCoroutinve;
            LabApi.Events.Handlers.PlayerEvents.GroupChanged += TryGiveCoroutinve;
            LabApi.Events.Handlers.PlayerEvents.Left += OnLeft;
            LabApi.Events.Handlers.ServerEvents.RoundRestarted += OnRoundRestarted;

            _registered = true;
        }

        internal static void Unregister()
        {
            if (!_registered)
                return;

            LabApi.Events.Handlers.PlayerEvents.Joined -= TryGiveCoroutinve;
            LabApi.Events.Handlers.PlayerEvents.GroupChanged -= TryGiveCoroutinve;
            LabApi.Events.Handlers.PlayerEvents.Left -= OnLeft;
            LabApi.Events.Handlers.ServerEvents.RoundRestarted -= OnRoundRestarted;

            _registered = false;
        }

        private static void TryGiveCoroutinve<T>(T ev) where T : IPlayerEvent => PlayerPrefix.GiveCoroutine(ev.Player);

        private static void OnLeft(PlayerLeftEventArgs ev) => PlayerPrefix.StopCoroutine(ev.Player);

        private static void OnRoundRestarted() => PlayerPrefix.StopAllCoroutines();
    }
}