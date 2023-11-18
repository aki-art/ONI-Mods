using UnityEngine;

namespace FUtility
{
	public class Tiles
	{
		public static string baseFolder = "assets/tiles";

		public static void AddCustomTileAtlas(BuildingDef def, string name, bool shiny = false, string referenceAtlas = "tiles_metal")
		{
			TextureAtlas reference = global::Assets.GetTextureAtlas(referenceAtlas);

			// base
			def.BlockTileAtlas = FAssets.GetCustomAtlas($"{name}_tiles", baseFolder, reference);

			// place
			def.BlockTilePlaceAtlas = FAssets.GetCustomAtlas($"{name}_tiles_place", baseFolder, reference);

			// specular
			if (shiny)
			{
				def.BlockTileShineAtlas = FAssets.GetCustomAtlas($"{name}_tiles_spec", baseFolder, reference);
			}
		}

		public static void AddCustomTileTops(BuildingDef def, string name, bool shiny = false, string decorInfo = "tiles_glass_tops_decor_info", string existingPlaceID = null, string existingSpecID = null)
		{
			var info = Object.Instantiate(global::Assets.GetBlockTileDecorInfo(decorInfo));

			// base
			if (info is object)
			{
				info.atlas = FAssets.GetCustomAtlas($"{name}_tiles_tops", baseFolder, info.atlas);
				def.DecorBlockTileInfo = info;
			}

			// placement
			if (existingPlaceID.IsNullOrWhiteSpace())
			{
				var placeInfo = Object.Instantiate(global::Assets.GetBlockTileDecorInfo(decorInfo));
				placeInfo.atlas = FAssets.GetCustomAtlas($"{name}_tiles_tops_place", baseFolder, placeInfo.atlas);
				def.DecorPlaceBlockTileInfo = placeInfo;
			}
			else
			{
				def.DecorPlaceBlockTileInfo = global::Assets.GetBlockTileDecorInfo(existingPlaceID);
			}

			// specular
			if (shiny)
			{
				string id = existingSpecID.IsNullOrWhiteSpace() ? $"{name}_tiles_tops_spec" : existingSpecID;
				info.atlasSpec = FAssets.GetCustomAtlas(id, baseFolder, info.atlasSpec);
			}
		}
	}
}
