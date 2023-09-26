using Backwalls.Buildings;
using Backwalls.CustomTile;
using FUtility;
using HarmonyLib;
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

						if (def.PrefabID == "DecorPackA_UnobtaniumAlloyStainedGlassTile")
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

				if (ModAssets.customTiles.additionalTiles != null)
				{
					var tileAtlas = Assets.GetTextureAtlas("tiles_metal");
					foreach (var tile in ModAssets.customTiles.additionalTiles)
					{
						var atlas = ScriptableObject.CreateInstance<TextureAtlas>();
						atlas.texture = tile.Value.textures[CustomTileLoader.MAIN];
						atlas.scaleFactor = tileAtlas.scaleFactor;
						atlas.items = tileAtlas.items;

						var uiSprite = SpriteHelper.GetSpriteForAtlas(atlas);

						Log.Debug($"loading pattern {tile.Key} with borderTag: {tile.Value.borderTag}");

						var name = Strings.TryGet(tile.Value.name, out var translatedName) ? translatedName.String : tile.Value.name;

						var pattern = new BackwallPattern(tile.Key, name, tile.Value.textures[CustomTileLoader.MAIN], uiSprite, 500)
						{
							BorderTag = tile.Value.borderTag ?? tile.Key
						};

/*						if (tile.Value.shaderSettings != null)
						{
							switch (tile.Value.shaderSettings.Name)
							{
								case CustomTileLoader.SHINY:
									if(tile.Value.textures.TryGetValue(CustomTileLoader.SHINY, out var shinyTex))
									{
										pattern.specularTexture = shinyTex;
										pattern.specularColor = Color.white;//tile.Value.shaderSettings.Data["specularColor"]
									}
									else
									{
										Log.Warning($"Shader Shiny defined for {pattern.ID}, but specular texture is null.");
									}
									break;
							}
						}*/

						Mod.variants.Add(tile.Key, pattern);
					}
				}
			}
		}
	}
}
