using BackgroundTiles.Buildings;
using FUtility;
using HarmonyLib;
using System.Collections.Generic;

namespace BackgroundTiles.Patches
{
    class BuildingConfigManagerPatch
    {
        [HarmonyPatch(typeof(BuildingConfigManager), "RegisterBuilding")]
        public static class BuildingConfigManager_RegisterBuilding_Patch
        {
            public static void Postfix(IBuildingConfig config, Dictionary<IBuildingConfig, BuildingDef> ___configTable)
            {
                if (___configTable.TryGetValue(config, out BuildingDef originalDef))
                {
                    if (IsFloorTile(originalDef)) {
                        Log.Debuglog("Registering ", originalDef.Tag);
                        BackgroundTilesRegistry.RegisterTile(config, originalDef);
                    }
                }
            }

            private static readonly Tag[] tags = new Tag[]
            {
                GameTags.FloorTiles,
                TagManager.Create("MosaicTile") // in case Moasic tile is not updated for someone, up to date version has proper tag
            };

            private static bool IsFloorTile(BuildingDef def)
            {
                return def.IsTilePiece && 
                    def.BuildingComplete.HasAnyTags(tags) && 
                    def.BlockTileAtlas != null;
            }
        }
    }
}
