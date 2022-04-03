using HarmonyLib;
using MoreMarbleSculptures.Components;

namespace MoreMarbleSculptures.Patches
{
    public class ArtablePatch
    {
        [HarmonyPatch(typeof(Artable), "OnSpawn")]
        public class Artable_OnSpawn_Patch
        {
            // restore stage
            // Deserialization order does not seem to be guaranteed and Artable may override my changes on deserialize call
            public static void Prefix(Artable __instance, ref string ___currentStage)
            {
                if (__instance.TryGetComponent(out ArtOverride artOverride) && !artOverride.overrideStage.IsNullOrWhiteSpace())
                {
                    ___currentStage = artOverride.overrideStage;
                }
            }
        }

        [HarmonyPatch(typeof(Artable), "SetStage")]
        public class Artable_SetStage_Patch
        {
            public static void Prefix(Artable __instance, string stage_id, ref string ___currentStage)
            {
                if (__instance.TryGetComponent(out ArtOverride artOverride))
                {
                    artOverride.UpdateOverride(stage_id);
                }
            }
        }
    }
}
