using FUtility;
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
            public static void Prefix(Artable __instance, ref string ___currentStage, string ___defaultArtworkId)
            {
                if (__instance.TryGetComponent(out ArtOverride artOverride) && !artOverride.overrideStage.IsNullOrWhiteSpace())
                {
                    var newStage = artOverride.overrideStage;

                    if (Db.GetArtableStages().TryGet(newStage) == null)
                    {
                        Log.Info(newStage + " was not found, trying to upgrade");
                        newStage = MarbleSculptureConfig.ID + "_" + newStage;
                    }

                    if (Db.GetArtableStages().TryGet(newStage) != null)
                    {
                        ___currentStage = newStage;
                    }
                }

                if (Db.GetArtableStages().TryGet(___currentStage) == null)
                {
                    Log.Info(___currentStage + " was still not found, resetting to default");
                    ___currentStage = ___defaultArtworkId;
                }
            }
        }
    }
}
