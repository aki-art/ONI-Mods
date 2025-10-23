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
				STRINGS.MISC.AKISEXTRATWITCHEVENTS_PIZZABOX.DESC,
				100f,
				true,
				Assets.GetAnim("aete_pizzaboxes_kanim"),
				"idle",
				Grid.SceneLayer.Creatures,
				EntityTemplates.CollisionShape.RECTANGLE,
				1,
				1.6f,
				false);

			var storage = prefab.AddComponent<Storage>();
			storage.capacityKg = 100f;
			storage.allowItemRemoval = true;
			storage.allowClearable = false;

			prefab.AddOrGet<PizzaBox>();

			prefab.AddOrGet<Prioritizable>();


			if (prefab.TryGetComponent(out PrimaryElement primaryElement))
				primaryElement.Temperature = GameUtil.GetTemperatureConvertedToKelvin(60, GameUtil.TemperatureUnit.Celsius);

			return prefab;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst) { }

		public void OnSpawn(GameObject inst)
		{
			if (inst.TryGetComponent(out KBatchedAnimController kbac))
				kbac.Play("idle", KAnim.PlayMode.Loop);

			Prioritizable.AddRef(inst);
			inst.AddOrGet<Prioritizable>().SetMasterPriority(new PrioritySetting(PriorityScreen.PriorityClass.basic, 1));
		}
	}
}
