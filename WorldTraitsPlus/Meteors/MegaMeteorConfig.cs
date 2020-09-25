using STRINGS;
using UnityEngine;

namespace WorldTraitsPlus.Meteors
{
	class MegaMeteorConfig : IEntityConfig
	{

		public static string ID = "WTP_MegaComet";
		public GameObject CreatePrefab()
		{
			GameObject gameObject = EntityTemplates.CreateEntity(ID, STRINGS.UI.SPACEDESTINATIONS.COMETS.MEGAMETEOR.NAME);

			gameObject.AddOrGet<SaveLoadRoot>();
			gameObject.AddOrGet<LoopingSounds>();

			Comet comet = gameObject.AddOrGet<Comet>();
			comet.massRange = new Vector2(11f, 30f);
			comet.temperatureRange = new Vector2(323.15f, 423.15f);
			comet.explosionOreCount = new Vector2I(10, 20);
			comet.entityDamage = 45;
			comet.totalTileDamage = 10f;
			comet.splashRadius = 10;
			comet.impactSound = "Meteor_Large_Impact";
			comet.flyingSoundID = 1;
			comet.explosionEffectHash = Tuning.FXHashes.MegaMeteorImpact;//SpawnFXHashes.MeteorImpactDirt;



			PrimaryElement primaryElement = gameObject.AddOrGet<PrimaryElement>();
			primaryElement.SetElement(SimHashes.Cuprite);
			primaryElement.Temperature = (comet.temperatureRange.x + comet.temperatureRange.y) / 2f;

			KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
			kbatchedAnimController.AnimFiles = new KAnimFile[]
			{
				Assets.GetAnim("megameteor_kanim")
			};

			kbatchedAnimController.isMovable = true;
			kbatchedAnimController.initialAnim = "fall_loop";
			kbatchedAnimController.initialMode = KAnim.PlayMode.Loop;
			kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
			gameObject.AddOrGet<KCircleCollider2D>().radius = 3f;
			gameObject.transform.localScale = new Vector3(3f, 3f, 3f);


			return gameObject;
		}

		public void OnPrefabInit(GameObject go) { }

		public void OnSpawn(GameObject go)
		{

			//go.GetComponent<Comet>().explosionEffectHash =
		}
	}


}
