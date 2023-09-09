using SoftLockBegone.Content.Scripts;
using UnityEngine;

namespace SoftLockBegone.Content
{
	public class SLBEntityConfig : IEntityConfig
	{
		public const string ID = "SoftLockBegone_PlaceHolderEntity";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				ID,
				"Placeholder",
				"...",
				1f,
				Assets.GetAnim("slb_missing_kanim"),
				"object",
				Grid.SceneLayer.Creatures,
				1,
				1,
				default);

			prefab.AddComponent<SLB_EntityComponent>();

			prefab.AddComponent<Storage>();
			prefab.AddComponent<Storage>();
			prefab.AddComponent<Storage>();

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
