using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using RemoteAdmin;
using System;

namespace ColorTag.Commands
{
    internal sealed class ColorCheck : ICommand
    {
        public string Command { get; } = "check";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "Check player settings";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = sender is PlayerCommandSender playerCommandSender
                ? Player.Get(playerCommandSender)
                : Server.Host;

            if (!player.HasPermissions(Plugin.PluginConfig.AdminRequirePermission))
            {
                response = Plugin.PluginConfig.Translation.DontHavePermissions
                    .Replace("%permission%", Plugin.PluginConfig.AdminRequirePermission);
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Using: colortag check (UserID)";
                return false;
            }

            if (!PlayerPrefix.TryGetValue(arguments.At(0), out PlayerPrefix info))
            {
                response = Plugin.PluginConfig.Translation.OtherNotFound;
                return false;
            }

            string text = $"{info.UserId}";

            foreach(string s in info.Colors)
                text += $"\n{s}";

            response = text;
            return true;
        }
    }
}
