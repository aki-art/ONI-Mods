using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes.PandoraSubEvents
{
	public class PandoraFireworksEvent(float weight, int minCount, int maxCount, float maxRadius, float duration) : PandoraEventBase(weight, duration)
	{
		private readonly int minCount = minCount, maxCount = maxCount;
		private readonly float maxRadius = maxRadius;

		public override Danger GetDanger() => Danger.None;

		public override void Run(PandorasBox box)
		{
			base.Run(box);
			var count = Random.Range(minCount, maxCount);
			SetSpawnTimer(duration / count);
		}

		public override void Spawn(float dt)
		{
			base.Spawn(dt);

			var offset = (Vector3)Random.insideUnitCircle * maxRadius;
			var fx = FXHelpers.CreateEffect("achievement_kanim", (box.transform.position + offset) with { z = Grid.GetLayerZ(Grid.SceneLayer.FXFront2) }, layer: Grid.SceneLayer.FXFront2);


			fx.Rotation = Random.Range(0.0f, 360.0f);
			fx.PlaySpeedMultiplier = 0.33f;
			fx.animScale *= Random.Range(0.9f, 2.0f);
			fx.TintColour = Random.ColorHSV(0, 1.0f, 0.3f, 0.8f, 0.7f, 0.9f);
			fx.Play("spark");
			fx.destroyOnAnimComplete = true;
		}
	}
}
