using FUtilityArt.Components;
using HarmonyLib;

namespace MoreMarbleSculptures.Patches
{
    public class ArtablePatch
    {
        // Here to maintain backwards compatibility
        [HarmonyPatch(typeof(Artable), "OnSpawn")]
        public class Artable_OnSpawn_Patch
        {
            public static void Prefix(Artable __instance, ref string ___currentStage)
            {
                if (__instance.TryGetComponent(out ArtOverride artOverride) && !artOverride.overrideStage.IsNullOrWhiteSpace())
                {
                    ___currentStage = artOverride.overrideStage;
                }
            }
        }

        [HarmonyPatch(typeof(Artable), "OnDeserialized")]
        public class Artable_OnDeserialized_Patch
        {
            // prevent invalid stages
            public static void Postfix(string ___defaultArtworkId, ref string ___currentStage)
            {
                if (Db.GetArtableStages().TryGet(___currentStage) == null)
                {
                    ___currentStage = ___defaultArtworkId;
                }
            }
        }
    }
}
