using HarmonyLib;

namespace ExtensionCords.Patches
{
    class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                FUtility.BuildingUtil.Buildings.RegisterBuildings(
                    typeof(Buildings.ExtensionCord.ExtensionCordOutletConfig),
                    typeof(Buildings.ExtensionCord.ExtensionCordReelConfig)
                    );
            }
        }
    }
}
