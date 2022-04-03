using FUtility;
using FUtilityArt;
using HarmonyLib;
using System.Linq;

namespace MoreMarbleSculptures.Patches
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
                var id = stage_id;
                if(!__instance.stages.Any(s => s.id == id)) 
                {
                    Log.Warning("MISSING STAGE " + stage_id);
                    stage_id = "Default";
                }

                ArtHelper.UpdateOverride(__instance, stage_id);
            }
        }
    }
}
