using CommandSystem;
using LabApi.Features.Wrappers;
using RemoteAdmin;
using System;
using System.Collections.Generic;

namespace ColorTag.Commands
{
    internal sealed class ColorRemove : ICommand
    {
        public string Command { get; } = "remove";
        public string[] Aliases { get; } = { };
        public string Description { get; } = "Remove colors";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = sender is PlayerCommandSender playerCommandSender
                ? Player.Get(playerCommandSender)
                : Server.Host;

            if (!PlayerPrefix.TryGetValue(player.UserId, out PlayerPrefix info))
            {
                response = Plugin.PluginConfig.Translation.NotFoundInDataBase;
                return false;
            }

            string text = string.Empty;

            List<string> colors = new List<string>();
            List<string> alreadyUsedColorsinforemove = info.Colors;

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
                alreadyUsedColorsinforemove.Remove(s);

            foreach (var s in alreadyUsedColorsinforemove)
                text += $"{s} ";

            info.Colors = alreadyUsedColorsinforemove;

            PlayerPrefix.Save(info);
            PlayerPrefix.GiveCoroutine(player);

            response = Plugin.PluginConfig.Translation.Successfull
                .Replace("%current%", text);
            return true;
        }
    }
}
