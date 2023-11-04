using FUtility;
using TUNING;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs.Buildings
{
	public class PuzzleDoorConfig : IBuildingConfig
	{
		public static string ID = "AkisExtraTwitchEvents_PuzzleDoor";

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				ID,
				1,
				2,
				"aete_puzzle_door_kanim",
				30,
				60f,
				BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
				MATERIALS.ALL_METALS,
				9999,
				BuildLocationRule.Anywhere,
				BUILDINGS.DECOR.PENALTY.TIER2,
				NOISE_POLLUTION.NONE);

			def.Overheatable = false;
			def.Repairable = false;
			def.Floodable = false;
			def.Invincible = true;
			def.Breakable = false;
			def.IsFoundation = true;
			def.TileLayer = ObjectLayer.FoundationTile;
			def.AudioCategory = AUDIO.CATEGORY.METAL;
			def.PermittedRotations = PermittedRotations.R90;
			def.SceneLayer = Grid.SceneLayer.Building;
			def.ForegroundLayer = Grid.SceneLayer.InteriorWall;
#if DEBUG
			def.ShowInBuildMenu = true;
#else
			def.ShowInBuildMenu = false;
#endif

			SoundEventVolumeCache.instance.AddVolume("door_manual_kanim", "ManualPressureDoor_gear_LP", NOISE_POLLUTION.NOISY.TIER1);
			SoundEventVolumeCache.instance.AddVolume("door_manual_kanim", "ManualPressureDoor_open", NOISE_POLLUTION.NOISY.TIER2);
			SoundEventVolumeCache.instance.AddVolume("door_manual_kanim", "ManualPressureDoor_close", NOISE_POLLUTION.NOISY.TIER2);

			Log.Debug("created puzzle oor prefab");

			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			/*			var door = go.AddOrGet<Door>();
						door.hasComplexUserControls = false;
						door.unpoweredAnimSpeed = 1f;
						door.doorType = Door.DoorType.Sealed;
			*/
			go.AddOrGet<ZoneTile>();
			//go.AddOrGet<AccessControl>();
			go.AddOrGet<PuzzleDoor2>();
			//go.AddOrGet<Unsealable>();
			go.AddOrGet<KBoxCollider2D>();

			Prioritizable.AddRef(go);

			//go.AddOrGet<Workable>().workTime = 5f;
			go.AddOrGet<KBatchedAnimController>().fgLayer = Grid.SceneLayer.BuildingFront;

			var primaryElement = go.GetComponent<PrimaryElement>();
			primaryElement.SetElement(SimHashes.Unobtanium);
			primaryElement.Temperature = 273f;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			//go.GetComponent<AccessControl>().controlEnabled = false;
			go.GetComponent<Deconstructable>().allowDeconstruction = false;
			go.GetComponent<KBatchedAnimController>().initialAnim = "closed";
		}
	}
}
