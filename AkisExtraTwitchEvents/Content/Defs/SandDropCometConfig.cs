using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class SandDropCometConfig : IEntityConfig
	{
		public const string ID = "AkisExtraTwitchEvents_SandComet";

		public GameObject CreatePrefab()
		{
			var go = EntityTemplates.CreateEntity(ID, global::STRINGS.ELEMENTS.SAND.NAME, true);

			go.AddOrGet<SaveLoadRoot>();
			go.AddOrGet<LoopingSounds>();

			var kbac = go.AddOrGet<KBatchedAnimController>();
			kbac.AnimFiles = [Assets.GetAnim("sand_kanim")];

			kbac.isMovable = true;
			kbac.initialAnim = "idle1";
			kbac.initialMode = KAnim.PlayMode.Loop;
			kbac.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;


			var falling = go.AddOrGet<FallingDebris>();
			falling.addTiles = 1;
			falling.tempMinC = 25f;
			falling.tempMaxC = 32;
			falling.massMin = 10f;
			falling.massMax = 25f;
			falling.speedMin = 4f;
			falling.speedMax = 5f;
			falling.spawnAngleMin = 55f;
			falling.spawnAngleMax = 60f;

			var primaryElement = go.AddOrGet<PrimaryElement>();
			primaryElement.SetElement(SimHashes.Sand);

			var collider = go.AddOrGet<KCircleCollider2D>();
			collider.radius = 0.5f;

			return go;
		}

		public string[] GetDlcIds() => null;

		public void OnPrefabInit(GameObject inst)
		{
		}

		public void OnSpawn(GameObject inst)
		{
		}
	}
}
