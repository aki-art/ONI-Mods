using Backwalls.Buildings;
using FUtility;
using HarmonyLib;
using System.IO;
using UnityEngine;

namespace Backwalls.Patches
{
    public class GeneratedBuildingsPatch
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.BASE, DecorativeBackwallConfig.ID, CONSTS.SUB_BUILD_CATEGORY.Base.TILES, ExteriorWallConfig.ID);
                ModUtil.AddBuildingToPlanScreen(CONSTS.BUILD_CATEGORY.BASE, SealedBackwallConfig.ID, CONSTS.SUB_BUILD_CATEGORY.Base.TILES, ExteriorWallConfig.ID);

                BuildingUtil.AddToResearch(DecorativeBackwallConfig.ID, CONSTS.TECH.DECOR.ARTISTRY);
                BuildingUtil.AddToResearch(SealedBackwallConfig.ID, CONSTS.TECH.DECOR.CLOTHING);
            }

            public static void Postfix()
            {
                Log.Debug("Adding variants");

                Integration.TravelTubeAnywhere.OnAllBuildingsLoaded();

                foreach (var def in Assets.BuildingDefs)
                {
                    var allowed = !def.BuildingComplete.HasTag(ModAssets.Tags.noBackwall) || def.PrefabID == TileConfig.ID;
                    if (def.BlockTileAtlas != null && allowed)
                    {
                        Mod.variants[def.PrefabID] = new BackwallPattern(def);

                        if(def.PrefabID == "DecorPackA_UnobtaniumAlloyStainedGlassTile")
                        {
                            Mod.variants[def.PrefabID].uniqueMaterial = ModAssets.rainbowTileMaterial;
                            Mod.variants[def.PrefabID].specularColor = Util.ColorFromHex("606060");
                        }
                    }
                }

                // Adds a solid color variant
                var sprite = Assets.GetBuildingDef(ExteriorWallConfig.ID).GetUISprite();
                var solidColor = new BackwallPattern("BlankPattern", "Solid Color", ModAssets.blankTileTex, sprite, 999)
                {
                    biomeTint = 0
                };

                Mod.variants.Add("BlankPattern", solidColor);

                if(ModAssets.customTiles.additionalTiles !=  null)
                {
                    var tileAtlas = Assets.GetTextureAtlas("tiles_metal");
                    foreach (var tile in ModAssets.customTiles.additionalTiles)
                    {
                        var atlas = ScriptableObject.CreateInstance<TextureAtlas>();
                        atlas.texture = tile.Value.texture;
                        atlas.scaleFactor = tileAtlas.scaleFactor;
                        atlas.items = tileAtlas.items;

                        var uiSprite = SpriteHelper.GetSpriteForDef(atlas);
                        var pattern = new BackwallPattern(tile.Key, tile.Value.name, tile.Value.texture, uiSprite, 500);

                        Mod.variants.Add(tile.Key, pattern);
                    }
                }

                FAssets.SaveImage(Assets.GetBuildingDef(TileConfig.ID).BlockTileAtlas.texture, Path.Combine(Utils.ModPath, "test", "tile atlas.png"));
            }
        }
    }
}
