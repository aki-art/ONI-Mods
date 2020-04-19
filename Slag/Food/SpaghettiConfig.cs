using STRINGS;
using System.Collections.Generic;
using UnityEngine;
using static ComplexRecipe;
using static EdiblesManager;

namespace Slag.Food
{
	class SpaghettiConfig : IEntityConfig
	{
		public const string ID = "Spaghetti";
		public static ComplexRecipe recipe;
		public GameObject CreatePrefab()
		{
			GameObject prefab = EntityTemplates.CreateLooseEntity(
				id: ID,
				name: "Spaghetti",
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
				quality: 4,
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