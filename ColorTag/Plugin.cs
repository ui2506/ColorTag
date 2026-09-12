using ColorTag.Configs;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Paths;
using LabApi.Loader.Features.Plugins;
using LiteDB;
using MEC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ColorTag
{
    public sealed class Plugin : Plugin<Config>
    {
        public override string Name { get; } = "ColorTag";
        public override string Author { get; } = "ui_2506";
        public override string Description { get; } = "Animated player tag colors";
        public override Version Version { get; } = new Version(2, 2, 0);
        public override Version RequiredApiVersion { get; } = new Version(1, 1, 7);

        internal static readonly Dictionary<Player, CoroutineHandle> PlayerCoroutines = new Dictionary<Player, CoroutineHandle>();

        internal static readonly Dictionary<string, string> AvailableColors = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
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

        private static readonly string AvailableColorsText = "Available colors: " + string.Join(", ", AvailableColors.Select(color => $"<color={color.Value}>{color.Key}</color>"));

        internal static Config PluginConfig { get; private set; }
        internal static LiteDatabase Data { get; private set; }

        public override void Enable()
        {
            PluginConfig = Config;

            string directory = Path.Combine(PathManager.Configs.FullName, "DataBase", "ColorTag");

            Directory.CreateDirectory(directory);

            Data = new LiteDatabase(Path.Combine(directory, $"ColorSetting{Server.Port}.db"));

            PlayerPrefix.Initialize();
            EventHandlers.Register();
        }

        public override void Disable()
        {
            EventHandlers.Unregister();
            Data?.Dispose();

            Data = null;
            PluginConfig = null;
        }

        internal static string ShowColors() => AvailableColorsText;
    }
}