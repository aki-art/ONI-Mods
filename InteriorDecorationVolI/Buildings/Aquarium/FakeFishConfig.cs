using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InteriorDecorationv1.Buildings.Aquarium
{
    class FakeFishConfig : IEntityConfig
    {
		public static string ID = ModAssets.PREFIX + "FakeFish";
		public GameObject CreatePrefab()
		{
			GameObject gameObject = EntityTemplates.CreateBasicEntity(
				id: ID,
				name: "Fish",
				desc: "blub",
				mass: 1f,
				unitMass: true,
				anim: Assets.GetAnim("pacu_kanim"),
				initialAnim: "fall",
				sceneLayer: Grid.SceneLayer.Backwall);

			EntityTemplates.AddCollision(gameObject, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f);
			gameObject.AddOrGet<FakeFish>();
			gameObject.AddOrGet<KSelectable>();

			return gameObject;
		}

		public void OnPrefabInit(GameObject inst)
		{
			inst.AddOrGet<OccupyArea>().OccupiedCellsOffsets = new CellOffset[1];
			inst.AddComponent<Klei.AI.Modifiers>();
			inst.AddOrGet<DecorProvider>().SetValues(TUNING.DECOR.BONUS.TIER2);
			inst.GetComponent<KBatchedAnimController>().PlayMode = KAnim.PlayMode.Loop;
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
