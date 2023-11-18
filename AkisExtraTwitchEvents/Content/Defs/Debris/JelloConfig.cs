using UnityEngine;

namespace Twitchery.Content.Defs.Debris
{
	public class JelloConfig : IOreConfig
	{
		public SimHashes ElementID => Elements.Jello;

		public GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateLiquidOreEntity(ElementID);
			ExtendEntityToFood(prefab, TFoodInfos.jello);

			return prefab;
		}

		public static GameObject ExtendEntityToFood(GameObject template, EdiblesManager.FoodInfo foodInfo)
		{
			if (template.TryGetComponent(out KPrefabID kPrefabID))
			{
				kPrefabID.AddTag(GameTags.Edible);
				template.AddOrGet<Edible>().FoodInfo = foodInfo;

				kPrefabID.instantiateFn += go => go.GetComponent<Edible>().FoodInfo = foodInfo;

				if (template.TryGetComponent(out PrimaryElement primaryElement))
				{
					primaryElement.MassPerUnit = 1f;
					primaryElement.Units = 1000f;
				}

				GameTags.DisplayAsCalories.Add(kPrefabID.PrefabTag);
			}
			return template;
		}
	}
}
