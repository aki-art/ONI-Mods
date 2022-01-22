using Asphalt.Buildings;
using FUtility.BuildingUtil;
using HarmonyLib;

namespace Asphalt.Patches
{
    class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                Buildings.RegisterBuildings(typeof(AsphaltTileConfig));
            }
        }
    }
}
