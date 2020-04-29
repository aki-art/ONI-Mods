using UnityEngine;
using STRINGS;
using static EdiblesManager;

namespace Slag.Food
{
	class CottonCandyConfig : IEntityConfig
	{
		public const string ID = "CottonCandy";
		// public static ComplexRecipe recipe;
		public GameObject CreatePrefab()
		{
			GameObject prefab = EntityTemplates.CreateLooseEntity(
				id: ID,
				name: SlagStrings.SLAGSTRINGS.ITEMS.FOOD.COTTON_CANDY.NAME,
				desc: ITEMS.FOOD.BURGER.DESC,
				mass: 1f,
				unitMass: false,
				anim: Assets.GetAnim("cottoncandy_kanim"),
				initialAnim: "object",
				sceneLayer: Grid.SceneLayer.Front,
				collisionShape: EntityTemplates.CollisionShape.RECTANGLE,
				width: 0.8f,
				height: 0.4f,
				isPickupable: true,
				sortOrder: 0,
				element: SimHashes.Creature,
				additionalTags: null);

			FoodInfo foodInfo = new FoodInfo(
				id: ID,
				caloriesPerUnit: 6000000f,
				quality: 3,
				preserveTemperatue: 255.15f,
				rotTemperature: 277.15f,
				spoilTime: 60f,
				can_rot: true);

			return EntityTemplates.ExtendEntityToFood(prefab, foodInfo);
		}

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
