/*using Twitchery.Content.Scripts.Worm;
using UnityEngine;

namespace Twitchery.Content.Defs
{
	public class SmallWormConfig : EntityConfigBase
	{
		public const string ID = "AkisExtraTwitchEvents_WormSmall";

		public override GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreateBasicEntity(
				ID,
				"Worm",
				"",
				1000f,
				false,
				Assets.GetAnim("farmtile_kanim"),
				"off",
				Grid.SceneLayer.FXFront2);

			var head = prefab.AddOrGet<WormHead>();
			head.segmentCount = 0;
			head.lifeTime = CONSTS.CYCLE_LENGTH * 5f;

			head.mawRadius = 1f;
			head.mawStrength = 0.1f;
			head.playerDetectionRadius = 15f;

			// simulation
			head.solidDamping = 3.77f; // very slidy so it "overshoots" targets
			head.airDamping = 7f;
			head.defaultSpeed = 15f;
			head.maximumSpeed = 0.17f;
			head.chaseSpeed = 0.25f;
			head.airSpeedMultiplier = 1.2f;
			head.mass = 1.3f;

			// meandering
			head.erraticFrequency = 0.41f;
			head.erraticMovementFactor = 8f;

			var kbac = prefab.GetComponent<KBatchedAnimController>();
			kbac.visibilityType = KAnimControllerBase.VisibilityType.Always;
			kbac.isMovable = true;
			if (kbac.visibilityListenerRegistered)
				kbac.UnregisterVisibilityListener();

			var collider = prefab.AddComponent<KCircleCollider2D>();
			collider.radius = 1f;

			var spriteRenderer = prefab.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = Assets.GetSprite("akisextratwitchevents_spice_goldflake");

			return prefab;
		}
	}
}
*/