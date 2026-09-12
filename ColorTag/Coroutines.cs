using LabApi.Features.Wrappers;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ColorTag
{
    internal static class Coroutines
    {
        internal static IEnumerator<float> ChangeColor(Player player, IEnumerable<string> colors)
        {
            yield return Timing.WaitForSeconds(0.1f);

            if (!colors.Any())
                yield break;

            int currentIndex = 0;
            float interval = Math.Max(0.1f, Plugin.PluginConfig.Interval);
            string[] colorList = colors.ToArray();

            while (!player.IsDestroyed)
            {
                if (currentIndex >= colorList.Length)
                    currentIndex = 0;

                player.GroupColor = colorList[currentIndex];

                currentIndex++;

                yield return Timing.WaitForSeconds(interval);
            }

            yield break;
        }
    }
}
