using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using RemoteAdmin;
using System;

namespace ColorTag.Commands
{
    internal sealed class ColorDelete : ICommand
    {
        public string Command { get; } = "delete";
        public string[] Aliases { get; } = { "del" };
        public string Description { get; } = "Delete player data";

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

            if (arguments.Count != 1)
            {
                response = "Using: colortag delete (UserID/all)";
                return false;
            }

            switch (arguments.At(0))
            {
                case "all":
                    if (!player.HasPermissions(Plugin.PluginConfig.DropDataRequirePermission))
                    {
                        response = Plugin.PluginConfig.Translation.DontHavePermissions
                            .Replace("%permission%", Plugin.PluginConfig.DropDataRequirePermission);
                        return false;
                    }

                    PlayerPrefix.DeleteAll();

                    response = Plugin.PluginConfig.Translation.KillDataBase;
                    return true;

                default:
                    if (!PlayerPrefix.TryGetValue(arguments.At(0), out PlayerPrefix info))
                    {
                        response = Plugin.PluginConfig.Translation.OtherNotFound;
                        return false;
                    }

                    PlayerPrefix.DeletePlayer(arguments.At(0));

                    response = Plugin.PluginConfig.Translation.SuccessfullDeleted
                        .Replace("%userid%", arguments.At(0));
                    return true;
            }
        }
    }
}
