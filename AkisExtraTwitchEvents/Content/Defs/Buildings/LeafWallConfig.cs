using TUNING;
using UnityEngine;

namespace Twitchery.Content.Defs.Buildings
{
	public class LeafWallConfig : IBuildingConfig
	{
		public const string ID = "AkisExtraTwitcEvents_LeafWall";

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				ID,
				1, 1,
				"aete_leafwall_kanim",
				30,
				3f,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,
				MATERIALS.RAW_MINERALS_OR_WOOD,
				1600f,
				BuildLocationRule.NotInTiles,
				new EffectorValues(10, 0),
				NOISE_POLLUTION.NONE);

			def.DebugOnly = true;
			def.Entombable = false;
			def.Floodable = false;
			def.Overheatable = false;
			def.AudioCategory = AUDIO.CATEGORY.PLASTIC;
			def.AudioSize = "small";
			def.BaseTimeUntilRepair = -1f;
			def.DefaultAnimState = "on";
			def.ObjectLayer = ObjectLayer.Backwall;
			def.SceneLayer = Grid.SceneLayer.Backwall;
			def.PermittedRotations = PermittedRotations.Unrotatable;
			def.ReplacementLayer = ObjectLayer.ReplacementBackwall;
			def.ReplacementCandidateLayers =
			[
				ObjectLayer.FoundationTile,
					ObjectLayer.Backwall
			];
			def.ReplacementTags =
			[
				GameTags.FloorTiles,
					GameTags.Backwall
			];

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
			go.AddComponent<ZoneTile>();
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.GetComponent<KPrefabID>().AddTag(GameTags.Backwall);
			GeneratedBuildings.RemoveLoopingSounds(go);
		}
	}
}
