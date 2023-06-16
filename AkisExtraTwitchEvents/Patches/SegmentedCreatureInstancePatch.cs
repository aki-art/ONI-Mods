/*using FUtility;
using HarmonyLib;
using Twitchery.Content;
using Twitchery.Content.Scripts;

namespace Twitchery.Patches
{
    public class SegmentedCreatureInstancePatch
    {
        [HarmonyPatch(typeof(SegmentedCreature.Instance), "CreateSegments")]
        public class SegmentedCreature_Instance_CreateSegments_Patch
        {
            public static void Prefix(SegmentedCreature.Instance __instance)
            {
                if (__instance.HasTag(TTags.longBoi))
                {
                    Log.Debuglog("Has wormy");
                    __instance.def.numBodySegments = 30;
                    __instance.def.numPathNodes = 30 * 3;
                }
                else
                {
                    Log.Debuglog("worm spawned, not long boi");
                }
            }
        }
    }
}
*/