using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

namespace Slag
{
    class SlagWoolConfig : IEntityConfig
	{
		public static string ID = "SlagWool";
		private readonly AttributeModifier overHeatModifier = new AttributeModifier(Db.Get().BuildingAttributes.OverheatTemperature.Id, +120);
		public GameObject CreatePrefab()
		{
			GameObject prefab = EntityTemplates.CreateLooseEntity(
				id: ID,
				name: "Slag Wool",
				desc: "Slag wool desc.",
				mass: 1f,
				unitMass: true,
				anim: Assets.GetAnim("swampreedwool_kanim"),
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
					GameTags.IndustrialIngredient,
					ModAssets.slagWoolTag
				});

			prefab.AddOrGet<EntitySplitter>();
			prefab.AddOrGet<PrefabAttributeModifiers>().AddAttributeDescriptor(overHeatModifier);

			return prefab;
		}

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
			//inst.GetComponent<KPrefabID>().AddTag(ModAssets.slagWoolTag, false);
		}

	}
}
