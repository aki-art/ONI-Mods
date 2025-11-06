using Twitchery.Content.Scripts;
using Twitchery.Content.Scripts.Worm;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class SmallWormConfig : EntityConfigBase
	{
		public const string ID = "AkisExtraTwitchEvents_SmallWorm";

		public override GameObject CreatePrefab()
		{
			var prefab = Object.Instantiate(ModAssets.Prefabs.smallWormHead);

			prefab.gameObject.SetActive(false);
			Object.DontDestroyOnLoad(prefab);

			EntityTemplates.ConfigEntity(prefab, ID, "Small Worm", true);

			var primaryElement = prefab.AddComponent<PrimaryElement>();
			primaryElement.SetElement(SimHashes.Creature);
			primaryElement.Temperature = 300f;
			primaryElement.Mass = 100f;

			prefab.AddComponent<SimTemperatureTransfer>();
			prefab.AddComponent<InfoDescription>().description = "Big bOi.";
			prefab.AddComponent<Notifier>();

			prefab.AddOrGet<KSelectable>().SetName("Small Worm");

			var kbac = prefab.AddOrGet<KBatchedAnimController>();
			kbac.AnimFiles = [Assets.GetAnim("aete_smallworm_kanim")];
			kbac.isMovable = false;
			kbac.initialAnim = "none";
			kbac.initialMode = KAnim.PlayMode.Paused;
			kbac.isVisible = false;

			prefab.AddComponent<SaveLoadRoot>();
			prefab.AddComponent<StateMachineController>();
			prefab.AddComponent<LoopingSounds>();


			prefab.AddTag(ONITwitchLib.ExtraTags.OniTwitchSurpriseBoxForceDisabled);
			prefab.AddTag(TTags.pandorasBoxSpawnableMild);

			var collider = prefab.AddComponent<KCircleCollider2D>();
			collider.radius = 1f;

			var head = prefab.AddComponent<WormHead>();
			head.lifeTime = CONSTS.CYCLE_LENGTH * 2f;
			head.approachPlayerRadius = 15f;

			head.playerDetectionRadius = 20f;
			head.trackButt = false;
			head.chaseEnergy = 10;

			// simulation
			head.solidDamping = 2.77f; // very slidy so it "overshoots" targets
			head.airDamping = 5f;
			head.defaultSpeed = 4f;
			head.maximumSpeed = 0.22f;
			head.chaseSpeed = 0.10f;
			head.airSpeedMultiplier = 1.1f;
			head.mass = 1.3f;

			head.noisePitch = 1.1f;
			head.noiseVolume = 1.2f;

			// meandering
			head.erraticFrequency = 0.41f;
			head.erraticMovementFactor = 8f;

			// segments
			head.segmentCount = 14;
			head.crumbDistance = 0.2f;
			head.segmentDistance = 0.7f;

			head.segmentPrefab = ModAssets.Prefabs.smallWormBody.transform;
			head.buttPrefab = ModAssets.Prefabs.smallWormButt.transform;

			// special fx
			head.cameraShakeFactor = 0f;

			var eater = prefab.AddComponent<TileEater>();
			eater.brushRadius = 1;
			eater.destroyTiles = false;
			eater.affectFoundation = true;
			eater.canBreakNeutronium = false;
			eater.strength = 0.15f;
			eater.entityDamage = 1f;
			eater.buildingDamage = 10;

			return prefab;
		}
	}
}
