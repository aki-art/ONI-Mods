using UnityEngine;

namespace WorldCreep.WorldEvents
{
	public class EarthQuakeConfig : IEntityConfig
    {
        public static string ID = Tuning.PREFIX + "EarthQuake";
		public GameObject CreatePrefab()
		{
			GameObject gameObject = EntityTemplates.CreatePlacedEntity(
				id: ID,
				name: STRINGS.DISASTERS.EARTHQUAKE.NAME,
				desc: STRINGS.DISASTERS.EARTHQUAKE.DESCRIPTION,
				mass: 1f,
				anim: Assets.GetAnim("pincher_kanim"),
				initialAnim: "slap_pre",
				sceneLayer: Grid.SceneLayer.FXFront,
				width: 2,
				height: 2,
				decor: default);


			KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
			kbatchedAnimController.isMovable = true;
			kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
			kbatchedAnimController.SetFGLayer(Grid.SceneLayer.FXFront);

			gameObject.AddComponent<EarthQuake>();
			gameObject.AddOrGet<LoopingSounds>();

			return gameObject;
		}

		public void OnPrefabInit(GameObject go) { }

		public void OnSpawn(GameObject go) { }
	}
}
