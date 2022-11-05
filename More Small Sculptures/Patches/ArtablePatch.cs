using HarmonyLib;
using MoreSmallSculptures.FUtilityArt;

namespace MoreSmallSculptures.Patches
{
    public class ArtablePatch
    {
        [HarmonyPatch(typeof(Artable), "OnSpawn")]
        public class Artable_OnSpawn_Patch
        {
            public static void Prefix(Artable __instance, ref string ___currentStage)
            {
                ArtHelper.RestoreStage(__instance, ref ___currentStage);
            }
        }

        [HarmonyPatch(typeof(Artable), "SetStage")]
        public class Artable_SetStage_Patch
        {
            public static void Prefix(Artable __instance, ref string stage_id)
            {
                ArtHelper.UpdateOverride(__instance, stage_id);
            }
        }
    }
}
