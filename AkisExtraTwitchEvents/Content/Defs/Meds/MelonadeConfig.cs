using System.Collections.Generic;
using UnityEngine;

namespace Twitchery.Content.Defs.Meds
{
	public class MelonadeConfig : IMultiEntityConfig
	{
		public const string ID = "AkisExtraTwitchEvents_melonade";

		public List<GameObject> CreatePrefabs()
		{
			var items = new List<GameObject>();

			if (Mod.isSgtChaosHere)
				items.Add(CreatePrefab());

			return items;
		}

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				STRINGS.ITEMS.PILLS.AKISEXTRATWITCHEVENTS_MELONADE.NAME,
				STRINGS.ITEMS.PILLS.AKISEXTRATWITCHEVENTS_MELONADE.DESC,
				1f,
				true,
				Assets.GetAnim("aete_melonade_kanim"),
				"object",
				Grid.SceneLayer.Creatures,
				EntityTemplates.CollisionShape.RECTANGLE,
				0.8f,
				0.8f,
				true);

			var med = new MedicineInfo(ID, "WarmTouch", MedicineInfo.MedicineType.Booster, null, []);

			EntityTemplates.ExtendEntityToMedicine(prefab, med);

			return prefab;
		}

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
