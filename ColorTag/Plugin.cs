using ColorTag.Configs;
using ColorTag.EventHandlers;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;
using LiteDB;
using MEC;
using System;
using System.Collections.Generic;
using System.IO;

namespace ColorTag
{
    public sealed class Plugin : Plugin<Config>
    {
        public override string Name { get; } = "ColorTag";
        public override string Author { get; } = "ui_2506";
        public override string Description { get; } = "ColorTag";
        public override Version Version { get; } = new Version(2, 2, 0);
        public override Version RequiredApiVersion { get; } = new Version(1, 1, 7);

        private string MainDirectory;
        private string ConfigDirectory;

        internal static readonly Dictionary<Player, CoroutineHandle> PlayerCoroutines = new Dictionary<Player, CoroutineHandle>();

        internal static readonly Dictionary<string, string> AvailableColors = new Dictionary<string, string>() 
        {
            { "pink", "#FF96DE" },
            { "red", "#C50000" },
            { "brown", "#944710" },
            { "silver", "#A0A0A0" },
            { "light_green", "#32CD32" },
            { "crimson", "#DC143C" },
            { "cyan", "#00B7EB" },
            { "aqua", "#00FFFF" },
            { "deep_pink", "#FF1493" },
            { "tomato", "#FF6448" },
            { "yellow", "#FAFF86" },
            { "magenta", "#FF0090" },
            { "blue_green", "#4DFFB8" },
            { "orange", "#FF9966" },
            { "lime", "#BFFF00" },
            { "green", "#228B22" },
            { "emerald", "#50C878" },
            { "carmine", "#960018" },
            { "nickel", "#727472" },
            { "mint", "#98FB98" },
            { "army_green", "#4B5320" },
            { "pumpkin", "#EE7600" }
        };

        internal static Config PluginConfig { get; private set; }
        internal static LiteDatabase Data { get; private set; }

        public override void Enable()
        {
            ReloadFiles();

            PluginConfig = Config;
            Data = new LiteDatabase($"{ConfigDirectory}/ColorSetting{Server.Port}.db");

            PlayerEvents.Register();
        }

        public override void Disable()
        {
            Data.Dispose();
            PlayerEvents.Unregister();

            PluginConfig = null;
            Data = null;
        }

        internal void ReloadFiles()
        {
            MainDirectory = GetParentDirectory() + "/configs" + "/DataBase";
            ConfigDirectory = MainDirectory + "/ColorTag";

            if (!Directory.Exists(MainDirectory))
            {
                Directory.CreateDirectory(MainDirectory);
                Logger.Warn("DataBase directory not found! Creating...");
            }

            if (!Directory.Exists(ConfigDirectory))
            {
                Directory.CreateDirectory(ConfigDirectory);
                Logger.Warn("ColorTag directory not found! Creating...");
            }
        }

        private string GetParentDirectory()
        {
            string parentPath = Path.GetDirectoryName(FilePath);

            for (int i = 0; i < 2; i++)
            {
                parentPath = Directory.GetParent(parentPath)?.FullName;

                if (parentPath == null)
                    throw new InvalidOperationException("It is impossible to go higher than the root directory.");
            }

            return parentPath;
        }

        internal static string ShowColors()
        {
            string content = string.Empty;

            foreach (var colors in AvailableColors)
            {
                content += $"<color={colors.Value}>{colors.Key}</color>" + "," + " ";
            }

            return $"Aviable colors: {content}";
        }
    }
}