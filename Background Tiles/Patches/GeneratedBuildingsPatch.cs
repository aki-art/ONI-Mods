using BackgroundTiles.Buildings;
using HarmonyLib;

namespace BackgroundTiles.Patches
{
    class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                BackgroundTilesRegistry.SetBaseTemplate();
            }
        }
    }
}
