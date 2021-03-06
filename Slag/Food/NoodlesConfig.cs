﻿using UnityEngine;
using STRINGS;
using static EdiblesManager;

namespace Slag.Food
{
	class NoodlesConfig : IEntityConfig
	{
		public const string ID = "Noodles";
		// public static ComplexRecipe recipe;
		public GameObject CreatePrefab()
		{
			GameObject prefab = EntityTemplates.CreateLooseEntity(
				id: ID,
				name: "Dry Noodles",
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
				quality: 1,
				preserveTemperatue: 255.15f,
				rotTemperature: 277.15f,
				spoilTime: 9600f,
				can_rot: false);

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