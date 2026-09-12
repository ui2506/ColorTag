using LabApi.Features.Wrappers;
using MEC;
using System.Collections.Generic;
using System.Linq;

namespace ColorTag
{
    internal static class Coroutines
    {
        internal static IEnumerator<float> ChangeColor(Player player, IEnumerable<string> colors)
        {
            string[] colorList = colors.ToArray();

            if (colorList.Length <= 0)
                yield break;

            int currentIndex = 0;

            while (player.ReferenceHub != null)
            {
                yield return Timing.WaitForSeconds(Plugin.PluginConfig.Interval);

                if (currentIndex >= colorList.Length)
                    currentIndex = 0;

                if (player != null && player.IsOnline)
                    player.GroupColor = colorList[currentIndex];

                currentIndex++;
            }

            yield break;
        }
    }
}
