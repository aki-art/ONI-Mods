using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class AETE_ManagerGameObjectConfig : IEntityConfig
	{
		public const string ID = "AETE_ManagerGameObject";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateEntity(ID, "Aete Manager", false);

			prefab.AddOrGet<SaveLoader>();

			prefab.AddComponent<AETE_SaveMod>();
			prefab.AddComponent<SolarStormManager>();

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
