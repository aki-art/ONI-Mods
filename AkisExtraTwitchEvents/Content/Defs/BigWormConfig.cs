using Twitchery.Content.Scripts;
using Twitchery.Content.Scripts.Worm;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class BigWormConfig : EntityConfigBase
	{
		public const string ID = "AkisExtraTwitchEvents_BigWorm";

		public override GameObject CreatePrefab()
		{
			var prefab = Object.Instantiate(ModAssets.Prefabs.bigWormHead);
			prefab.gameObject.SetActive(false);
			Object.DontDestroyOnLoad(prefab);

			EntityTemplates.ConfigEntity(prefab, ID, "Death laser", true);

			var primaryElement = prefab.AddComponent<PrimaryElement>();
			primaryElement.SetElement(SimHashes.Creature);
			primaryElement.Temperature = 300f;
			primaryElement.Mass = 100f;

			prefab.AddComponent<SimTemperatureTransfer>();
			prefab.AddComponent<InfoDescription>().description = "Big bOi.";
			prefab.AddComponent<Notifier>();

			prefab.AddOrGet<KSelectable>().SetName("Giant Worm");

			var kbac = prefab.AddOrGet<KBatchedAnimController>();
			kbac.AnimFiles = [Assets.GetAnim("barbeque_kanim")];
			kbac.isMovable = false;
			kbac.initialAnim = "none";
			kbac.initialMode = KAnim.PlayMode.Paused;
			kbac.isVisible = false;

			prefab.AddComponent<SaveLoadRoot>();
			prefab.AddComponent<StateMachineController>();
			prefab.AddComponent<LoopingSounds>();


			prefab.AddTag(ONITwitchLib.ExtraTags.OniTwitchSurpriseBoxForceDisabled);

			var collider = prefab.AddComponent<KCircleCollider2D>();
			collider.radius = 3f;

			var head = prefab.AddComponent<WormHead>();
			head.lifeTime = CONSTS.CYCLE_LENGTH * 5f;
			head.approachPlayerRadius = 10f;

			head.mawRadius = 2f;
			head.mawStrength = 0.1f;
			head.playerDetectionRadius = 20f;
			head.trackButt = true;

			// simulation
			head.solidDamping = 3.77f; // very slidy so it "overshoots" targets
			head.airDamping = 7f;
			head.defaultSpeed = 21f;
			head.maximumSpeed = 0.24f;
			head.chaseSpeed = 0.33f;
			head.airSpeedMultiplier = 1.1f;
			head.mass = 1.3f;

			// meandering
			head.erraticFrequency = 0.41f;
			head.erraticMovementFactor = 8f;

			// segments
			head.segmentCount = 50;
			head.crumbDistance = 0.51f;
			head.segmentDistance = 2f;
			head.segmentPrefab = ModAssets.Prefabs.bigWormBody.transform;

			// special fx
			head.cameraShakeFactor = 1f;

			var eater = prefab.AddComponent<TileEater>();
			eater.brushRadius = 2;
			eater.affectFoundation = true;
			eater.canBreakNeutronium = false;
			eater.strength = byte.MaxValue;

			return prefab;
		}
	}
}
