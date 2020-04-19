using Harmony;
using UnityEngine;

namespace Slag.Buildings
{
    class BuildingPatches
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                string InsulatedtileID = DenseInsulationTileConfig.ID.ToUpperInvariant();
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{InsulatedtileID}.NAME", "Dense Insulation Tile");
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{InsulatedtileID}.EFFECT", "Dense insulation effect");
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{InsulatedtileID}.DESC", "Dense insulation desc.");

                ModUtil.AddBuildingToPlanScreen("Base", DenseInsulationTileConfig.ID);
            }
        }

/*        [HarmonyPatch(typeof(Rendering.BlockTileRenderer))]
        [HarmonyPatch("GetCellColour")]
        public static class BlockTileRenderer_GetCellColour_Patch
        {
            public static void Postfix(int cell, SimHashes element, ref Color __result)
            {
                var building = Grid.Objects[cell, (int)ObjectLayer.FoundationTile];
                if (building != null)
                {
                    var tags = building.GetComponent<KPrefabID>().Tags;
                    Log.Info(building.name + " " + element);

                    if (building.HasTag("GlassTile") && element == ModAssets.slagGlassSimHash)
                    {
                        __result = Color.cyan;
                    }
                }
            }
        }*/
    }
}
