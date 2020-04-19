using UnityEngine;
using STRINGS;
using static EdiblesManager;
using System.Collections.Generic;
using static ComplexRecipe;

namespace Slag.Food
{
	class SeafoodPastaConfig : IEntityConfig
	{
		public const string ID = "SeafoodPasta";
		public static ComplexRecipe recipe;
		public GameObject CreatePrefab()
		{
			GameObject prefab = EntityTemplates.CreateLooseEntity(
				id: ID,
				name: "Seafood Pasta",
				desc: ITEMS.FOOD.BURGER.DESC,
				mass: 1f,
				unitMass: false,
				anim: Assets.GetAnim("frost_burger_kanim"),
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
				quality: 6,
				preserveTemperatue: 255.15f,
				rotTemperature: 277.15f,
				spoilTime: 600f,
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