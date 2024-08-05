using DecorPackB.Content.Db;
using DecorPackB.Content.Scripts;
using FUtility;
using TUNING;
using UnityEngine;
using static FUtility.CONSTS;

namespace DecorPackB.Content.Defs.Buildings
{
	internal class GiantFossilDisplayConfig : IBuildingConfig
	{
		public static string ID = Mod.PREFIX + "GiantFossilDisplay";

		public override BuildingDef CreateBuildingDef()
		{
			Log.Assert("rule", Db.ModDb.BuildLocationRules.GiantFossilRule);
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				7,
				6,
				"decorpackb_giantfossil_default_kanim",
				BUILDINGS.HITPOINTS.TIER2,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
				new[]
				{
					800f,
					50f,
					1f
				},
				new[]
				{
					DPTags.fossilMaterial.ToString(),
					SimHashes.Steel.ToString(),
					DPTags.buildingFossilNodule.ToString(),
				},
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				Db.ModDb.BuildLocationRules.GiantFossilRule,
				DECOR.BONUS.TIER5,
				NOISE_POLLUTION.NONE
			);

			def.Floodable = false;
			def.Overheatable = false;
			def.AudioCategory = AUDIO_CATEGORY.PLASTIC;
			def.BaseTimeUntilRepair = -1f;
			def.ViewMode = OverlayModes.Decor.ID;
			def.DefaultAnimState = "base";
			def.PermittedRotations = PermittedRotations.FlipH;

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddTag(GameTags.Decoration);
			go.AddTag(DPTags.fossilBuilding);
			go.AddOrGet<BuildingComplete>().isArtable = true;
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddComponent<GiantFossilDisplay>();
			go.AddComponent<GiantExhibition>();
			go.AddComponent<FoundationOrAnchorMonitor>();
			AddVisualizer(go, Color.black, false);
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			base.DoPostConfigurePreview(def, go);
			AddVisualizer(go, Color.white, true);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			AddVisualizer(go, Color.white, false);
		}

		private static void AddVisualizer(GameObject go, Color cableColor, bool updatePosition)
		{
			var gameObject = new GameObject("cable visualizer");
			gameObject.transform.parent = go.transform;
			gameObject.transform.SetLocalPosition(Vector3.zero);

			var cables = gameObject.AddComponent<GiantFossilCableVisualizer>();
			cables.updatePositionEveryFrame = updatePosition;
			cables.color = cableColor;
			cables.linePrefab = ModAssets.Prefabs.cablePrefab;
		}
	}
}
