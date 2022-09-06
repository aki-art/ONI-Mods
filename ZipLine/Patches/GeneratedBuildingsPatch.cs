using FUtility;
using HarmonyLib;
using ZipLine.Content.Buildings.ZiplinePost;

namespace ZipLine.Patches
{
    internal class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.BASE, ZiplinePostConfig.ID, Consts.SUB_BUILD_CATEGORY.Base.TUBES, TravelTubeConfig.ID);
                //BuildingUtil.AddToResearch(AsphaltTileConfig.ID, Consts.TECH.POWER.IMPROVED_COMBUSTION);
            }
        }
    }
}
