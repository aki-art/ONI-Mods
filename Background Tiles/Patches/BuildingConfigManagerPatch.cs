using FUtility;
using HarmonyLib;
using System.Collections.Generic;

namespace BackgroundTiles.Patches
{
    public class BuildingConfigManagerPatch
    {
        [HarmonyPatch(typeof(BuildingConfigManager), "OnPrefabInit")]
        public static class BuildingConfigManager_OnPrefabInit_Patch
        {
            // Happens once per game launch, persistent between world loads
            public static void Prefix(BuildingConfigManager __instance)
            {
                __instance.gameObject.AddComponent<BackgroundTilesManager>();
            }
        }


        [HarmonyPatch(typeof(BuildingConfigManager), "RegisterBuilding")]
        public static class BuildingConfigManager_RegisterBuilding_Patch
        {
            public static void Postfix(IBuildingConfig config, Dictionary<IBuildingConfig, BuildingDef> ___configTable)
            {
                if (___configTable.TryGetValue(config, out BuildingDef originalDef))
                {
                    if (IsFloorTile(originalDef))
                    {
                        Log.Debuglog("Registering ", originalDef.Tag);
                        BackgroundTilesManager.Instance.RegisterTile(config, originalDef);
                    }
                    else if (IsBackwall(originalDef))
                    {
                        originalDef.BuildingComplete.AddTag(ModAssets.Tags.backWall);
                    }
                }
            }

            private static readonly Tag[] tags = new Tag[]
            {
                GameTags.FloorTiles,
                TagManager.Create("MosaicTile") // in case Moasic tile is not updated for someone, up to date version has proper tag
            };

            private static bool IsBackwall(BuildingDef def)
            {
                return def.IsTilePiece &&
                    def.BuildingComplete.GetComponent<ZoneTile>() != null &&
                    def.WidthInCells == 1 && def.HeightInCells == 1 &&
                    def.SceneLayer == Grid.SceneLayer.Backwall;
            }

            private static bool IsFloorTile(BuildingDef def)
            {
                return def.IsTilePiece &&
                    def.BuildingComplete.HasAnyTags(tags) &&
                    !def.BuildingComplete.HasTag(ModAssets.Tags.noBackwall) &&
                    def.BlockTileAtlas != null;
            }
        }
    }
}
