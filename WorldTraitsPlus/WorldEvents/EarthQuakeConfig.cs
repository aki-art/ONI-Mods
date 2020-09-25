using UnityEngine;

namespace WorldTraitsPlus.WorldEvents
{
	class EarthQuakeConfig : IEntityConfig
	{
		public static string ID = "WTP_EarthQuake";

		public GameObject CreatePrefab()
		{
			GameObject gameObject = EntityTemplates.CreateEntity(ID, STRINGS.DISASTERS.EARTHQUAKE.NAME, true);
			gameObject.AddComponent<EarthQuake>();

			KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
			kbatchedAnimController.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("earthquake_kanim")
			};

			kbatchedAnimController.isMovable = true;
			kbatchedAnimController.initialAnim = "rumbling";
			kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
			kbatchedAnimController.SetFGLayer(Grid.SceneLayer.FXFront);
			kbatchedAnimController.SetSceneLayer(Grid.SceneLayer.FXFront);

			gameObject.AddOrGet<LoopingSounds>();

			return gameObject;
		}

		public void OnPrefabInit(GameObject go) { }

		public void OnSpawn(GameObject go) { }
	}
}
