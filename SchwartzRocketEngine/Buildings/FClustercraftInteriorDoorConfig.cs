using System.Collections.Generic;
using TUNING;
using UnityEngine;
using static STRINGS.BUILDINGS.PREFABS;

namespace SchwartzRocketEngine.Buildings
{
    public class FClustercraftInteriorDoorConfig : IEntityConfig
	{
		public static string ID = Mod.Prefix("FClustercraftInteriorDoor");

        public string[] GetDlcIds() => DlcManager.AVAILABLE_EXPANSION1_ONLY;

        public GameObject CreatePrefab()
		{

            GameObject gameObject = EntityTemplates.CreatePlacedEntity(
                ID,
                CLUSTERCRAFTINTERIORDOOR.NAME,
                CLUSTERCRAFTINTERIORDOOR.DESC,
				400f,
                Assets.GetAnim("rocket_hatch_door_kanim"), 
				"closed",
                Grid.SceneLayer.TileFront, 
				1, 
				2,
                BUILDINGS.DECOR.BONUS.TIER0,
                NOISE_POLLUTION.NOISY.TIER0,
                SimHashes.Creature, 
				 new List<Tag>
                {
                    GameTags.Gravitas
				}, 293f);

			gameObject.AddTag(GameTags.NotRoomAssignable);

			PrimaryElement primaryElement = gameObject.GetComponent<PrimaryElement>();
			primaryElement.SetElement(SimHashes.Unobtanium, true);
			primaryElement.Temperature = 294.15f;

			gameObject.AddOrGet<Operational>();

			gameObject.AddOrGet<LoopingSounds>();

			gameObject.AddOrGet<Prioritizable>();

			KBatchedAnimController kbac = gameObject.AddOrGet<KBatchedAnimController>();
			kbac.sceneLayer = Grid.SceneLayer.BuildingBack;
			kbac.fgLayer = Grid.SceneLayer.BuildingFront;

            FClustercraftInteriorDoor door = gameObject.AddOrGet<FClustercraftInteriorDoor>();
            door.bgAnim = "spaceball_bg_kanim";
			door.bgOffset = new Vector3(12.5f, 4f, 20f);

			gameObject.AddOrGet<AssignmentGroupController>().generateGroupOnStart = false;

			gameObject.AddOrGet<NavTeleporter>().offset = new CellOffset(1, 0);

			gameObject.AddOrGet<AccessControl>();

			return gameObject;
		}

		public void OnPrefabInit(GameObject inst)
		{
			inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[]
			{
				ObjectLayer.Building
			};
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
