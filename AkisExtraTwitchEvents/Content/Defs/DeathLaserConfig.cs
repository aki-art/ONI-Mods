using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class DeathLaserConfig : IEntityConfig
	{
		public const string ID = "AETE_DeathLaser";

		public GameObject CreatePrefab()
		{
			var prefab = Object.Instantiate(ModAssets.Prefabs.deathLaser);
			prefab.gameObject.SetActive(false);
			Object.DontDestroyOnLoad(prefab);

			EntityTemplates.ConfigEntity(prefab, ID, "Death laser", false);

			var kbac = prefab.AddOrGet<KBatchedAnimController>();
			kbac.AnimFiles = [Assets.GetAnim("barbeque_kanim")];
			kbac.isMovable = false;
			kbac.initialAnim = "none";
			kbac.initialMode = KAnim.PlayMode.Paused;
			kbac.isVisible = false;

			prefab.AddComponent<SaveLoadRoot>();
			prefab.AddComponent<StateMachineController>();

			prefab.AddTag(ONITwitchLib.ExtraTags.OniTwitchSurpriseBoxForceDisabled);
			prefab.AddOrGet<DeathLaser>();

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst) { }
	}
}
