using FUtility;
using System;
using System.Collections.Generic;

namespace Moonlet.Utils
{
	public class EntityUtil
	{
		public static Dictionary<string, Type> mappings = [];

		private static readonly Dictionary<string, string> tileTopsLayoutLookup = new()
		{
			{ TileConfig.ID, "tiles_solid_tops_info" },
			{ GlassTileConfig.ID, "tiles_glass_tops_decor_info" },
			{ TilePOIConfig.ID, "tiles_POI_tops_decor_info" },
			{ BunkerTileConfig.ID, "tiles_bunker_tops_decor_info" },
			{ MetalTileConfig.ID, "tiles_metal_tops_decor_info" },
			{ MeshTileConfig.ID, "tiles_metal_tops_decor_info" },
			{ GasPermeableMembraneConfig.ID, "tiles_mesh_tops_decor_info" }
		};

		public static void AddCustomTileTops(BuildingDef def, string name, string place, string topsSpec, string baseFolder, bool shiny = false, string decorInfo = "tiles_glass_tops_decor_info", string existingPlaceID = null)
		{
			if (tileTopsLayoutLookup.TryGetValue(decorInfo, out var actualDecorInfo))
				decorInfo = actualDecorInfo;

			BlockTileDecorInfo original = Assets.GetBlockTileDecorInfo(decorInfo);

			if (original == null)
			{
				Log.Error($"{name}: Not a valid tile tops layout: {decorInfo}");
				return;
			}

			BlockTileDecorInfo info = UnityEngine.Object.Instantiate(original);

			// base
			if (info != null)
			{
				info.atlas = FAssets.GetCustomAtlas(name, baseFolder, info.atlas);
				def.DecorBlockTileInfo = info;
			}

			// placement
			if (existingPlaceID.IsNullOrWhiteSpace())
			{
				var placeInfo = UnityEngine.Object.Instantiate(Assets.GetBlockTileDecorInfo(decorInfo));
				placeInfo.atlas = FAssets.GetCustomAtlas(place, baseFolder, placeInfo.atlas);
				def.DecorPlaceBlockTileInfo = placeInfo;
			}
			else
			{
				def.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo(existingPlaceID);
			}

			// specular
			if (shiny && !topsSpec.IsNullOrWhiteSpace())
			{
				info.atlasSpec = FAssets.GetCustomAtlas(topsSpec, baseFolder, info.atlasSpec);
			}
		}
	}
}
