using FUtility;
using HarmonyLib;
using MoreMarbleSculptures.Components;
using System.Linq;

namespace MoreMarbleSculptures.Patches
{
    public class ArtablePatch
    {
        [HarmonyPatch(typeof(Artable), "OnSpawn")]
        public class Artable_OnSpawn_Patch
        {
            // restore stage
            public static void Prefix(Artable __instance, ref string ___currentStage)
            {
                if (__instance.TryGetComponent(out ArtOverride artOverride) && !artOverride.overrideStage.IsNullOrWhiteSpace())
                {
                    Log.Debuglog("RESTORING STAGE " + artOverride.overrideStage);
                    ___currentStage = artOverride.overrideStage;
                }
            }
        }

        [HarmonyPatch(typeof(Artable), "SetStage")]
        public class Artable_SetStage_Patch
        {
            // clear out invalid stages
            public static void Prefix(Artable __instance, ref string stage_id)
            {
                var id = stage_id;
                if (!__instance.stages.Any(s => s.id == id))
                {
                    Log.Warning($"Invalid stage ID {id} on {__instance.GetProperName()}, reset to default.");
                    stage_id = "Default";
                }
            }

            public static void Postfix(Artable __instance, string stage_id, ref string ___currentStage)
            {
                if (__instance.TryGetComponent(out ArtOverride artOverride))
                {
                    artOverride.OverrideAnim(stage_id);
                }

                __instance.Trigger((int)ModHashes.ArtableStangeChanged, stage_id);
            }
        }
    }
}
