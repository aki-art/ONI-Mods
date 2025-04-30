using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class SnowDropCometConfig : IEntityConfig
	{
		public const string ID = "AkisExtraTwitchEvents_SnowComet";

		public GameObject CreatePrefab()
		{
			var go = EntityTemplates.CreateEntity(ID, global::STRINGS.UI.SPACEARTIFACTS.OFFICEMUG.NAME, true);

			go.AddOrGet<SaveLoadRoot>();
			go.AddOrGet<LoopingSounds>();

			var kbac = go.AddOrGet<KBatchedAnimController>();
			kbac.AnimFiles = [Assets.GetAnim("snow_kanim")];
			kbac.isMovable = true;
			kbac.initialAnim = "idle1";
			kbac.initialMode = KAnim.PlayMode.Loop;
			kbac.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;


			var falling = go.AddOrGet<FallingDebris>();
			falling.addTiles = 1;
			falling.tempMinC = -15f;
			falling.tempMaxC = -5f;
			falling.massMin = 1f;
			falling.massMax = 2.5f;
			falling.speedMin = 2f;
			falling.speedMax = 4f;
			falling.spawnAngleMin = 55f;
			falling.spawnAngleMax = 60f;

			var primaryElement = go.AddOrGet<PrimaryElement>();
			primaryElement.SetElement(SimHashes.Snow);

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
