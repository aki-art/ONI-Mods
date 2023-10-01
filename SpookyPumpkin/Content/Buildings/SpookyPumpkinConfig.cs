using TUNING;
using UnityEngine;

namespace SpookyPumpkinSO.Content.Buildings
{
	public class SpookyPumpkinConfig : IBuildingConfig
	{
		public static string ID = "SP_SpookyPumpkin";
		private const int LUX = 700;
		private const float RANGE = 2f;

		public override BuildingDef CreateBuildingDef()
		{
			var ores = MATERIALS.RAW_METALS[0];
			var pumpkin = ModAssets.buildingPumpkinTag.ToString();

			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				1,
				"sp_spookypumpkin_kanim",
				BUILDINGS.HITPOINTS.TIER2,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER3,
				new float[] { 100f, 1f },
				new string[] { ores, pumpkin },
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				BuildLocationRule.OnFloor,
				BUILDINGS.DECOR.BONUS.TIER3,
				NOISE_POLLUTION.NONE);

			def.AudioCategory = AUDIO.CATEGORY.GLASS;
			def.ViewMode = OverlayModes.Decor.ID;
			def.PermittedRotations = PermittedRotations.FlipH;

			def.RequiresPowerInput = true;
			def.EnergyConsumptionWhenActive = 3f;
			def.SelfHeatKilowattsWhenActive = 0.2f;

			return def;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			var lightShapePreview = go.AddComponent<LightShapePreview>();
			lightShapePreview.lux = LUX;
			lightShapePreview.radius = RANGE;
			lightShapePreview.shape = LightShape.Circle;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			var prefabId = go.GetComponent<KPrefabID>();
			prefabId.AddTag(RoomConstraints.ConstraintTags.LightSource);
			prefabId.AddTag(RoomConstraints.ConstraintTags.Decor20);
			prefabId.AddTag(GameTags.Decoration);

			go.AddOrGet<CopyBuildingSettings>().copyGroupTag = ID;
			go.AddOrGet<Spooks>();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<EnergyConsumer>();
			go.AddOrGet<LoopingSounds>();

			var light2d = go.AddOrGet<Light2D>();
			light2d.overlayColour = LIGHT2D.FLOORLAMP_OVERLAYCOLOR;
			light2d.Color = new Color(2, 1.5f, 0.3f, 1);
			light2d.Range = RANGE;
			light2d.shape = LightShape.Circle;
			light2d.Offset = new Vector2(0, 0.5f);
			light2d.drawOverlay = true;
			light2d.Lux = LUX;

			go.AddOrGetDef<LightController.Def>();
		}

		public override string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;
	}
}
