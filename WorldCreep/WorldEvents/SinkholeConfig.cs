using UnityEngine;

namespace WorldCreep.WorldEvents
{
	public class SinkHoleConfig : IEntityConfig
	{
		public static string ID = Tuning.PREFIX + "SinkHole";
		public GameObject CreatePrefab()
		{
			GameObject gameObject = EntityTemplates.CreatePlacedEntity(
				id: ID,
				name: STRINGS.DISASTERS.EARTHQUAKE.NAME,
				desc: STRINGS.DISASTERS.EARTHQUAKE.DESCRIPTION,
				mass: 1f,
				anim: Assets.GetAnim("pacu_kanim"), // placeholder
				initialAnim: "falling",
				sceneLayer: Grid.SceneLayer.FXFront,
				width: 2,
				height: 2,
				decor: default);


			KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
			kbatchedAnimController.isMovable = true;
			kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
			kbatchedAnimController.SetFGLayer(Grid.SceneLayer.FXFront);

			gameObject.AddComponent<SinkHole>();
			gameObject.AddOrGet<LoopingSounds>();

			return gameObject;
		}

		public void OnPrefabInit(GameObject go) { }

		public void OnSpawn(GameObject go) { }
	}
}
