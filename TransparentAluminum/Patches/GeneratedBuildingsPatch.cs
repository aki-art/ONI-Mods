using FUtility;
using HarmonyLib;
using TransparentAluminum.Content.Buildings.SolarRoad;

namespace TransparentAluminum.Patches
{
    public class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.POWER, SolarRoadConfig.ID, Consts.SUB_BUILD_CATEGORY.Power.GENERATORS, SolarPanelConfig.ID);

                //BuildingUtil.AddToResearch(MoodLampConfig.ID, Consts.TECH.DECOR.INTERIOR_DECOR);
            }
        }
    }
}
