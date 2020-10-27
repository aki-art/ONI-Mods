using FUtility;
using TUNING;
using UnityEngine;

namespace TransparentAluminium
{
	class SolarPanelRoadConfig : IBuildingConfig, IModdedBuilding
	{
		public const string ID = "TAT_SolarPanelRoad";

		public MBInfo Info => new MBInfo(ID, "Base");

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 1,
				height: 1,
				anim: "floor_glass_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER1,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
				construction_mass: new float[] { BUILDINGS.CONSTRUCTION_MASS_KG.TIER4[0] },
				construction_materials: new string[] { "TransparentAluminum" },
				melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER3,
				build_location_rule: BuildLocationRule.Tile,
				decor: BUILDINGS.DECOR.PENALTY.TIER2,
				noise: NOISE_POLLUTION.NONE
				);

			BuildingTemplates.CreateFoundationTileDef(def);
			def.Floodable = false;
			def.Overheatable = false;
			def.Entombable = false;
			def.UseStructureTemperature = false;

			def.GeneratorWattageRating = 380f;
			def.GeneratorBaseCapacity = def.GeneratorWattageRating;
			def.ExhaustKilowattsWhenActive = 0.0f;
			def.SelfHeatKilowattsWhenActive = 0.0f;

			def.AudioCategory = "Metal";
			def.AudioSize = "small";
			def.BaseTimeUntilRepair = -1f;
			def.SceneLayer = Grid.SceneLayer.TileMain;
			def.isKAnimTile = true;
			def.BlockTileIsTransparent = true;
			def.BlockTileMaterial = Assets.GetMaterial("tiles_solid");

			TextureAtlas referenceAtlas = Assets.GetTextureAtlas("tiles_metal");
			def.BlockTileAtlas = ModAssets.GetCustomAtlas("tiles_transparent_aluminum", referenceAtlas);
			def.BlockTilePlaceAtlas = ModAssets.GetCustomAtlas("tiles_transparent_aluminum_place", referenceAtlas);
			def.BlockTileShineAtlas = ModAssets.GetCustomAtlas("tiles_transparent_aluminum_spec", referenceAtlas);

			BlockTileDecorInfo decorBlockTileInfo = Object.Instantiate(Assets.GetBlockTileDecorInfo("tiles_glass_tops_decor_info"));
			decorBlockTileInfo.atlas = ModAssets.GetCustomAtlas("tiles_transparent_aluminum_tops", decorBlockTileInfo.atlas);
			decorBlockTileInfo.atlasSpec = ModAssets.GetCustomAtlas("tiles_transparent_aluminum_tops_spec", decorBlockTileInfo.atlas);
			def.DecorBlockTileInfo = decorBlockTileInfo;
			def.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_glass_tops_decor_place_info");

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);

			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

			SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
			simCellOccupier.doReplaceElement = true;
			simCellOccupier.strengthMultiplier = 10f;
			simCellOccupier.notifyOnMelt = true;
			simCellOccupier.setTransparent = true;

			go.AddOrGet<TileTemperature>();
			go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = TileConfig.BlockTileConnectorID;
			go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RemoveLoopingSounds(go);
			var kprefabID = go.GetComponent<KPrefabID>();
			kprefabID.AddTag(GameTags.FloorTiles);
			kprefabID.AddTag(GameTags.Bunker);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			base.DoPostConfigureUnderConstruction(go);
			go.AddOrGet<KAnimGridTileVisualizer>();
			go.AddOrGet<Repairable>().expectedRepairTime = 52.5f;
			go.AddOrGetDef<PoweredActiveController.Def>();
		}
	}
}
