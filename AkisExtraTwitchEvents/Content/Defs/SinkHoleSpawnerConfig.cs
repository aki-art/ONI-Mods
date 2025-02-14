using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	internal class SinkHoleSpawnerConfig : IEntityConfig
	{
		public const string ID = "AETE_SinkHoleSpawner";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateEntity(ID, "Sink Hole", true);
			var sinkHole = prefab.AddComponent<AETE_SinkHole>();
			sinkHole.duration = 20f;
			sinkHole.radius = 7;

			var kbac = prefab.AddOrGet<KBatchedAnimController>();
			kbac.AnimFiles =
			[
				Assets.GetAnim("barbeque_kanim")
			];

			kbac.isMovable = true;
			kbac.initialAnim = "none";
			kbac.initialMode = KAnim.PlayMode.Paused;
			kbac.isVisible = false;

			prefab.AddOrGet<LoopingSounds>();

			prefab.AddOrGet<ItemSucker>();

			prefab.AddTag(ONITwitchLib.ExtraTags.OniTwitchSurpriseBoxForceDisabled);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
