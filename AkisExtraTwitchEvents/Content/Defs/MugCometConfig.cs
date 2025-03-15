using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class MugCometConfig : IEntityConfig
	{
		public const string ID = "AkisExtraTwitchEvents_MugComet";

		public GameObject CreatePrefab()
		{
			var go = EntityTemplates.CreateEntity(ID, global::STRINGS.UI.SPACEARTIFACTS.OFFICEMUG.NAME, true);

			go.AddOrGet<SaveLoadRoot>();
			go.AddOrGet<LoopingSounds>();

			var comet = go.AddOrGet<MugComet>();
			comet.affectedByDifficulty = false;
			comet.canHitDuplicants = true;
			comet.spawnVelocity = new Vector2(1.2f, 1.5f);
			comet.entityDamage = 1;
			comet.EXHAUST_ELEMENT = SimHashes.CarbonDioxide;
			comet.EXHAUST_RATE = 5f;
			comet.splashRadius = 0;
			comet.impactSound = "Meteor_Small_Impact";
			comet.flyingSoundID = 0;
			comet.explosionEffectHash = SpawnFXHashes.MeteorImpactLightDust;
			comet.explosionOreCount = new Vector2I(1, 1);
			comet.temperatureRange = new Vector2(296.15f, 318.15f);
			comet.chanceToBreak = 0.8f;
			comet.massRange = new Vector2(25f, 25f);
			comet.addTiles = 0;
			comet.totalTileDamage = 0f;
			comet.splashRadius = 1;
			comet.angleVariation = 0f;
			comet.craterPrefabs =
			[
				"artifact_OfficeMug"
			];

			var primaryElement = go.AddOrGet<PrimaryElement>();
			primaryElement.SetElement(SimHashes.Ceramic);

			var kbac = go.AddOrGet<KBatchedAnimController>();
			kbac.AnimFiles =
			[
				Assets.GetAnim("aete_mug_meteor_kanim")
			];

			kbac.isMovable = true;
			kbac.initialAnim = "fall_loop";
			kbac.initialMode = KAnim.PlayMode.Loop;
			kbac.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;

			var collider = go.AddOrGet<KCircleCollider2D>();
			collider.radius = 0.5f;

			go.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
			go.AddTag(GameTags.Comet);

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
