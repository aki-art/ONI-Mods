using FUtility;
using HarmonyLib;

namespace SchwartzRocketEngine.Patches
{
    public class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                FUtility.BuildingUtil.Buildings.RegisterBuildings(
                    typeof(Buildings.SandComberConfig));

#if DEBUG
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.ROCKETRY, RocketWallTileConfig.ID);
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.ROCKETRY, RocketEnvelopeWindowTileConfig.ID);

                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.ROCKETRY, RocketInteriorGasOutputPortConfig.ID);
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.ROCKETRY, RocketInteriorGasInputPortConfig.ID);

                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.ROCKETRY, RocketInteriorLiquidInputPortConfig.ID);
                ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.ROCKETRY, RocketInteriorLiquidOutputPortConfig.ID);

               // ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.ROCKETRY, ClustercraftInteriorDoorConfig.ID);

               // ModUtil.AddBuildingToPlanScreen(Consts.BUILD_CATEGORY.ROCKETRY, Buildings.FClustercraftInteriorDoorConfig.ID);
#endif
            }

        }
    }
}
