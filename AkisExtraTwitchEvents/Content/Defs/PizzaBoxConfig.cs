using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class PizzaBoxConfig : IEntityConfig
	{
		public const string ID = "AkisExtraTwitchEvents_PizzaBox";

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLooseEntity(
				ID,
				STRINGS.MISC.AKISEXTRATWITCHEVENTS_PIZZABOX.NAME,
				STRINGS.MISC.AKISEXTRATWITCHEVENTS_PIZZABOX.NAME,
				100f,
				true,
				Assets.GetAnim("aete_pizzaboxes_kanim"),
				"idle",
				Grid.SceneLayer.Creatures,
				EntityTemplates.CollisionShape.RECTANGLE,
				1,
				1.6f,
				true);

			var storage = prefab.AddComponent<Storage>();
			storage.capacityKg = 100f;
			storage.allowItemRemoval = true;

			prefab.AddOrGet<PizzaBox>();

			if (prefab.TryGetComponent(out PrimaryElement primaryElement))
				primaryElement.Temperature = GameUtil.GetTemperatureConvertedToKelvin(60, GameUtil.TemperatureUnit.Celsius);

			return prefab;
		}

		public string[] GetDlcIds() => DlcManager.AVAILABLE_ALL_VERSIONS;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst)
		{
			if (inst.TryGetComponent(out KBatchedAnimController kbac))
				kbac.Play("idle", KAnim.PlayMode.Loop);
		}
	}
}
