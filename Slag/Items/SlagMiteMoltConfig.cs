using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slag.Items
{
    class SlagMiteMoltConfig : IEntityConfig
	{
		public static string ID = "SlagMiteMolt"; 

		public GameObject CreatePrefab()
		{
			GameObject prefab = EntityTemplates.CreateLooseEntity(
				id: ID,
				name: "Slag Wool",
				desc: "Slag wool desc.",
				mass: 1f,
				unitMass: true,
				anim: Assets.GetAnim("slagwool_kanim"),
				initialAnim: "object",
				sceneLayer: Grid.SceneLayer.BuildingBack,
				collisionShape: EntityTemplates.CollisionShape.RECTANGLE,
				width: 0.8f,
				height: 0.45f,
				isPickupable: true,
				sortOrder: 0,
				element: ModAssets.slagSimHash,
				additionalTags: new List<Tag>(1) { GameTags.Miscellaneous });

			prefab.AddOrGet<EntitySplitter>();
			return prefab;
		}

		public void OnPrefabInit(GameObject inst)
		{
		}
		public void OnSpawn(GameObject inst)
		{
			inst.AddComponent<MiteMolt>();
		}

	}
}
