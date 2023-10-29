using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class TentaclePortalConfig : IEntityConfig
	{
		public const string ID = "AkisExtraTwitchEvents_TentaclePortal";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateEntity(ID, "Perfectly Normal Phenomenon", true);

			prefab.AddComponent<SaveLoadRoot>();
			prefab.AddComponent<StateMachineController>();

			prefab.AddTag(ONITwitchLib.ExtraTags.OniTwitchSurpriseBoxForceDisabled);

			prefab.AddOrGet<InfoDescription>().description = "A perfectly normal phenomenon, with nothing interesting or unusual about it.";

			EntityTemplates.AddCollision(prefab, EntityTemplates.CollisionShape.CIRCLE, 1, 1);

			var kbac = prefab.AddComponent<KBatchedAnimController>();

			kbac.AnimFiles = new[]
			{
				Assets.GetAnim("aete_tentacleportal_kanim")
			};

			kbac.sceneLayer = Grid.SceneLayer.BuildingFront;
			kbac.initialAnim = "anomaly_pre";
			kbac.initialMode = KAnim.PlayMode.Paused;

			var portal = prefab.AddComponent<RegularPipSpawnerPortal>();
			portal.tentacleSpawnChance = 0.24f;
			portal.portalOpenTimeSeconds = 3f;

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
