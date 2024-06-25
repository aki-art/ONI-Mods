using Moonlet.Scripts.Moonlet.Entities;
using UnityEngine;

namespace Moonlet.Scripts
{
	public class TileBuildingConfig : GenericBuildingConfig
	{
		public bool replacesElement;
		public float strengthMultiplier;
		public float speedMultiplier;
		public bool isBunker;
		public bool isTransparent;

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

			go.AddOrGet<TileTemperature>();
			go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = MeshTileConfig.BlockTileConnectorID;
			go.AddOrGet<BuildingHP>().destroyOnDamaged = true;

			var simCellOccupier = go.AddOrGet<SimCellOccupier>();
			simCellOccupier.doReplaceElement = replacesElement;
			simCellOccupier.strengthMultiplier = strengthMultiplier;
			simCellOccupier.movementSpeedMultiplier = speedMultiplier;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RemoveLoopingSounds(go);
			var kPrefabID = go.GetComponent<KPrefabID>();
			kPrefabID.AddTag(GameTags.FloorTiles);

			if (isBunker)
				kPrefabID.AddTag(GameTags.Bunker);

			if (isTransparent)
				kPrefabID.AddTag(GameTags.Transparent);
			go.AddComponent<SimTemperatureTransfer>();
			go.AddComponent<ZoneTile>();
		}
	}
}
