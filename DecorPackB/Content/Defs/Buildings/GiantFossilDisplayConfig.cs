using DecorPackB.Content.ModDb;
using DecorPackB.Content.Scripts;
using DecorPackB.Content.Scripts.BigFossil;
using TUNING;
using UnityEngine;
using static FUtility.CONSTS;

namespace DecorPackB.Content.Defs.Buildings
{
	internal class GiantFossilDisplayConfig : IBuildingConfig
	{
		public static string ID = "DecorPackB_GiantFossilDisplay";

		public override BuildingDef CreateBuildingDef()
		{
			var def = BuildingTemplates.CreateBuildingDef(
				ID,
				7,
				6,
				"decorpackb_giantfossil_default_kanim",
				BUILDINGS.HITPOINTS.TIER2,
				BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
				[
					800f,
					50f,
					1f
				],
				[
					DPTags.fossilMaterial.ToString(),
					SimHashes.Steel.ToString(),
					DPTags.buildingFossilNodule.ToString(),
				],
				BUILDINGS.MELTING_POINT_KELVIN.TIER1,
				ModDb.ModDb.BuildLocationRules.GiantFossilRule,
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
			go.AddComponent<InspireAll>().effectId = DPEffects.INSPIRED_GIANT;
			ConfigureCables(go, Color.black, false);

			var def = go.GetComponent<Building>().Def;
			ConfigureCables(def.BuildingPreview, Color.white, true);
			ConfigureCables(def.BuildingUnderConstruction, Color.white, false);
		}

		private static void ConfigureCables(GameObject go, Color color, bool alwaysUpdate)
		{
			go.AddComponent<BigFossil>().alwaysUpdate = alwaysUpdate;
			go.AddComponent<AnchorMonitor>();
			go.AddComponent<BigFossilCablesRenderer>().baseColor = color;
		}
	}
}
