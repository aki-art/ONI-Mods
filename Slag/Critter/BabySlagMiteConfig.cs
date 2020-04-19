using UnityEngine;

namespace Slag.Critter
{
	class BabySlagMiteConfig : IEntityConfig
	{
		public const string ID = "MiteBaby";
		public GameObject CreatePrefab()
		{
			GameObject prefab = SlagMiteConfig.CreateMite(
				ID,
				"Baby Slag Mite",
				"Baby Slag Mite desc.",
				"baby_hatch_kanim",
				true);
			EntityTemplates.ExtendEntityToBeingABaby(prefab, SlagMiteConfig.ID, null);
			return prefab;
		}

		public void OnPrefabInit(GameObject prefab)
		{
		}
		public void OnSpawn(GameObject inst)
		{
		}
	}
}

