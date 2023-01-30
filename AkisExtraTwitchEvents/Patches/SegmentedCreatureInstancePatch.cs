using FUtility;
using HarmonyLib;
using Twitchery.Content;

namespace Twitchery.Patches
{
    public class SegmentedCreatureInstancePatch
    {
        [HarmonyPatch(typeof(SegmentedCreature.Instance), "CreateSegments")]
        public class SegmentedCreature_Instance_CreateSegments_Patch
        {
            public static void Prefix(SegmentedCreature.Instance __instance)
            {
                if (__instance.master.gameObject.HasTag(TTags.longBoi))
                {
                    __instance.def.numBodySegments = 30;
                    __instance.def.numPathNodes = 30 * 3;
                }
            }
        }
    }
}
