using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using RemoteAdmin;
using System;
using System.Collections.Generic;

namespace ColorTag.Commands
{
    internal sealed class ColorAdd : ICommand
    {
        public string Command { get; } = "add";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "Add color";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = sender is PlayerCommandSender playerCommandSender 
                ? Player.Get(playerCommandSender) 
                : Server.Host;

            if (!player.HasPermissions(Plugin.PluginConfig.ColorRequirePermission))
            {
                response = Plugin.PluginConfig.Translation.DontHavePermissions
                    .Replace("%permission%", Plugin.PluginConfig.ColorRequirePermission);
                return false;
            }

            if (!PlayerPrefix.TryGetValue(player.UserId, out PlayerPrefix info))
            {
                response = Plugin.PluginConfig.Translation.NotFoundInDataBase;
                return false;
            }

            if (!Plugin.PluginConfig.GroupColorLimit.TryGetValue(player.UserGroup.Name, out int limit))
                limit = Plugin.PluginConfig.DefaultColorLimit;

            if (arguments.Count + info.Colors.Count > limit)
            {
                response = Plugin.PluginConfig.Translation.ColorLimit
                    .Replace("%limit%", limit.ToString());
                return false;
            }

            string text = string.Empty;

            List<string> colors = new List<string>();
            List<string> alreadyUsedColors = info.Colors;

            foreach (string arg in arguments)
            {
                if (!Plugin.AvailableColors.ContainsKey(arg))
                {
                    response = Plugin.PluginConfig.Translation.InvalidColor
                        .Replace("%arg%", arg)
                        .Replace("%colors%", Plugin.ShowColors());
                    return false;
                }

                colors.Add(arg);
            }

            foreach (var s in colors)
                alreadyUsedColors.Add(s);

            foreach (var s in alreadyUsedColors)
                text += $"{s} ";

            info.Colors = alreadyUsedColors;

            PlayerPrefix.Save(info);
            PlayerPrefix.GiveCoroutine(player);

            response = Plugin.PluginConfig.Translation.Successfull
                .Replace("%current%", text);
            return true;
        }
    }
}
