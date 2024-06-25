using FUtility;
using System;
using System.Collections.Generic;

namespace Moonlet.Utils
{
	public class EntityUtil
	{
		public static Dictionary<string, Type> mappings = new()
		{
			// components
			/*			{ "tag:yaml.org,2002:sublimates", typeof(SublimatesComponent) },
						{ "tag:yaml.org,2002:edible", typeof(EdibleComponent) },
						{ "tag:yaml.org,2002:lightEmitter", typeof(LightEmitterComponent) },
						{ "tag:yaml.org,2002:radiationEmitter", typeof(RadiationEmitterComponent) },
						{ "tag:yaml.org,2002:storage", typeof(StorageComponent) },
						{ "tag:yaml.org,2002:demolishable", typeof(DemolishableComponent) },
						{ "tag:yaml.org,2002:rummagable", typeof(RummagableComponent) },
						{ "tag:yaml.org,2002:simpleGenerator", typeof(SimpleGeneratorComponent) },
						{ "tag:yaml.org,2002:diggable", typeof(DiggableComponent) },
						{ "tag:yaml.org,2002:foundationMonitor", typeof(FoundationMonitorComponent) },
						{ "tag:yaml.org,2002:fabricator", typeof(FabricatorComponent) },

						// commands
						{ "tag:yaml.org,2002:destroy", typeof(DestroyCommand) },
						{ "tag:yaml.org,2002:spawnitems", typeof(SpawnItemsCommand) },
						{ "tag:yaml.org,2002:spawnelement", typeof(SpawnElementCommand) },
						{ "tag:yaml.org,2002:playRandom", typeof(RandomAnimationCommand) },
						{ "tag:yaml.org,2002:play", typeof(PlayAnimationCommand) },
						{ "tag:yaml.org,2002:dropMaterials", typeof(DropMaterialsCommand) },*/
		};

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
