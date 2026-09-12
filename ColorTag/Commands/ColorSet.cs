using CommandSystem;
using LabApi.Features.Permissions;
using LabApi.Features.Wrappers;
using System;
using System.Collections.Generic;

namespace ColorTag.Commands
{
    internal sealed class ColorSet : ICommand
    {
        public string Command { get; } = "set";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "Set colors";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Player.TryGet(sender, out Player player))
            {
                response = "Only player can run this command!";
                return false;
            }

            if (!player.HasPermissions(Plugin.PluginConfig.ColorRequirePermission))
            {
                response = Plugin.PluginConfig.Translation.DontHavePermissions
                    .Replace("%permission%", Plugin.PluginConfig.ColorRequirePermission);
                return false;
            }

            if (arguments.Count < 1)
            {
                response = "Usage: colortag set (colors)\n" + Plugin.ShowColors();
                return false;
            }

            if (!Plugin.PluginConfig.GroupColorLimit.TryGetValue(player.UserGroup.Name, out int limit))
                limit = Plugin.PluginConfig.DefaultColorLimit;

            if (arguments.Count > limit)
            {
                response = Plugin.PluginConfig.Translation.ColorLimit
                    .Replace("%limit%", limit.ToString());
                return false;
            }

            List<string> colors = new List<string>();

            foreach (string arg in arguments)
            {
                if (string.IsNullOrEmpty(arg))
                {
                    response = "Один из переданных цветов пустой или некорректный.";
                    return false;
                }

                if (!Plugin.AvailableColors.ContainsKey(arg))
                {
                    response = Plugin.PluginConfig.Translation.InvalidColor
                        .Replace("%arg%", arg)
                        .Replace("%colors%", Plugin.ShowColors());
                    return false;
                }

                colors.Add(arg);
            }

            string text = string.Empty;

            foreach (var s in colors)
                text += $"{s} ";

            if (!PlayerPrefix.TryGetValue(player.UserId, out PlayerPrefix info))
            {
                PlayerPrefix.SetColors(player, colors);
            }
            else
            {
                info.Colors = colors;
                PlayerPrefix.Save(info);
            }

            PlayerPrefix.GiveCoroutine(player);

            response = Plugin.PluginConfig.Translation.Successfull
                .Replace("%current%", text);
            return true;
        }
    }
}
