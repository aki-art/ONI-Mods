using FUtility;
using HarmonyLib;
using System;
using System.Reflection;

namespace PrintingPodRecharge.Integration.TwitchIntegration
{
    public class GeyserPatch
    {
        public static void Patch(Harmony harmony)
        {
            var targetMethod = typeof(Geyser).GetMethod("OnSpawn", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            var postfix = typeof(GeyserPatch).GetMethod("Postfix", new Type[] { typeof(Geyser) });

            harmony.Patch(targetMethod, null, new HarmonyMethod(postfix));
        }

        // Making geysers spawned on top of printing pods demolishable
        public static void Postfix(Geyser __instance)
        {
            var telepad = GameUtil.GetTelepad(__instance.GetMyWorldId());

            var overlapping = false;

            if (telepad != null)
            {
                var geyserArea = __instance.FindComponent<OccupyArea>();
                telepad.GetComponent<Building>().RunOnArea(cell =>
                {
                    if (geyserArea.CheckIsOccupying(cell))
                    {
                        overlapping = true;
                        return;
                    }
                });
            }

            if (overlapping)
            {
                __instance.FindOrAddComponent<Demolishable>();

                Log.Info($"Made the newly spawned {Util.StripTextFormatting(__instance.GetProperName())} near " +
                    $"{Util.StripTextFormatting(__instance.GetMyWorld().GetProperName())}'s Printing Pod demolishable.");
            }
        }
    }
}