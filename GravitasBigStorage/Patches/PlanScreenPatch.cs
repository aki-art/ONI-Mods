using GravitasBigStorage.Content;
using HarmonyLib;

namespace GravitasBigStorage.Patches
{
    public class PlanScreenPatch
    {
        [HarmonyPatch(typeof(PlanBuildingToggle), "StandardDisplayFilter")]
        public class PlanBuildingToggle_StandardDisplayFilter_Patch
        {
            public static void Prefix(ref bool ___researchComplete, BuildingDef ___def)
            {
                if (___def.PrefabID == GravitasBigStorageConfig.ID)
                {
                    ___researchComplete = GravitasBigStorageUnlockManager.Instance.hasUnlockedTech;
                }
            }
        }

        [HarmonyPatch(typeof(PlanScreen), "GetTooltipForRequirementsState")]
        public class PlanScreen_GetTooltipForRequirementsState_Patch
        {
            public static void Postfix(BuildingDef def, PlanScreen.RequirementsState state, ref string __result)
            {
                if(def.PrefabID == GravitasBigStorageConfig.ID && state == GravitasBigStorageUnlockManager.needsAnalysis)
                {
                    __result = string.Format(global::STRINGS.UI.PRODUCTINFO_REQUIRESRESEARCHDESC, global::STRINGS.BUILDINGS.PREFABS.LONELYMINIONHOUSE_COMPLETE.NAME);
                }
            }
        }

        [HarmonyPatch(typeof(PlanBuildingToggle), "CheckResearch")]
        public class PlanBuildingToggle_CheckResearch_Patch
        {
            public static void Postfix(BuildingDef ___def, ref bool ___researchComplete)
            {
                if(___researchComplete && ___def.PrefabID == GravitasBigStorageConfig.ID)
                {
                    ___researchComplete = GravitasBigStorageUnlockManager.Instance.hasUnlockedTech;
                }
            }
        }
    }
}
