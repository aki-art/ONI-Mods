using AETNTweaks.Buildings.PyrositePylon;
using FUtility;
using HarmonyLib;

namespace AETNTweaks.Patches
{
    public class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.UTILITIES, PyrositeResonatorChamberConfig.ID, Consts.SUB_BUILD_CATEGORY.Utilities.SPECIAL);

                BuildingUtil.AddToResearch(PyrositeResonatorChamberConfig.ID, Consts.TECH.DECOR.INTERIOR_DECOR);
            }

            public static void Postfix()
            {
                BuildingDef AETNDef = Assets.GetBuildingDef(MassiveHeatSinkConfig.ID);

                LightShapePreview lightPreview = AETNDef.BuildingPreview.AddComponent<LightShapePreview>();
                lightPreview.lux = 1200;
                lightPreview.radius = 5;
                lightPreview.shape = LightShape.Circle;
                lightPreview.offset = new CellOffset(0, 1);
            }
        }
    }
}
