using UnityEngine;

namespace Twitchery.Content.Defs.Meds
{
	public class LemonadeConfig : IEntityConfig
	{
		public const string ID = "AkisExtraTwitchEvents_Lemonade";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				STRINGS.ITEMS.PILLS.AKISEXTRATWITCHEVENTS_LEMONADE.NAME,
				STRINGS.ITEMS.PILLS.AKISEXTRATWITCHEVENTS_LEMONADE.DESC,
				1f,
				true,
				Assets.GetAnim("aete_lemonade_kanim"),
				"object",
				Grid.SceneLayer.Creatures,
				EntityTemplates.CollisionShape.RECTANGLE,
				0.8f,
				0.6f,
				true);

			var med = new MedicineInfo(ID, "RefreshingTouch", MedicineInfo.MedicineType.Booster, null, []);

			EntityTemplates.ExtendEntityToMedicine(prefab, med);

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
