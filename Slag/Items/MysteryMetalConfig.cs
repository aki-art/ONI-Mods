using ProcGen;
using Slag.Critter;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Slag.Items
{
	class MysteryMetalConfig : IEntityConfig
	{
		public static string ID = "MysteryMetal";
		public static SimHashes chosenElement;
		private static List<WeightedMetalOption> refinedMetalOptions;

		public GameObject CreatePrefab()
		{
			GameObject prefab = EntityTemplates.CreateLooseEntity(
				id: ID,
				name: "Mystery Metal",
				desc: "a mystery.",
				mass: 1f,
				unitMass: true,
				anim: Assets.GetAnim("mystery_metal_kanim"),
				initialAnim: "object",
				sceneLayer: Grid.SceneLayer.BuildingBack,
				collisionShape: EntityTemplates.CollisionShape.RECTANGLE,
				width: 0.8f,
				height: 0.45f,
				isPickupable: true,
				sortOrder: 0,
				element: ModAssets.slagSimHash,
				additionalTags: new List<Tag>
				{
					GameTags.MiscPickupable
				});

			prefab.AddOrGet<EntitySplitter>();

			return prefab;
		}

		public void OnPrefabInit(GameObject inst)
		{
/*			refinedMetalOptions = new List<WeightedMetalOption>()
				{
					new WeightedMetalOption(SimHashes.Aluminum, .8f),
					new WeightedMetalOption(SimHashes.Copper, .8f),
					new WeightedMetalOption(SimHashes.Gold, .3f),
					new WeightedMetalOption(SimHashes.Iron, 1f),
					new WeightedMetalOption(SimHashes.Lead, .5f),
					new WeightedMetalOption(SimHashes.Niobium, .02f),
					new WeightedMetalOption(SimHashes.Steel, .05f),
					new WeightedMetalOption(SimHashes.TempConductorSolid, .01f),
					new WeightedMetalOption(SimHashes.Tungsten, .03f),
				};*/

		}

		public void OnSpawn(GameObject inst)
		{
			// chose a random metal
			refinedMetalOptions = ItemPatches.GetMetalRewards("exquisite");
			chosenElement = WeightedRandom.Choose(refinedMetalOptions, ModAssets.miteRandom).element;

			Log.Debuglog($"Spawned a mystery ore, the chosen element is {chosenElement}");

			// spawn the random ore
			var original = inst.GetComponent<PrimaryElement>();
			var element = ElementLoader.FindElementByHash(chosenElement);

			var result = element.substance.SpawnResource(
				position: inst.transform.position,
				mass: original.Mass,
				temperature: original.Temperature,
				disease_idx: original.DiseaseIdx,
				disease_count: original.DiseaseCount,
				prevent_merge: false,
				forceTemperature: false,
				manual_activation: false);

			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, element.name, result.transform);

			// self destruct
			Util.KDestroyGameObject(inst);
		}
	}
}
