using UnityEngine;

namespace Twitchery.Content
{
	public class MugComet : Comet, ISim33ms
	{
		private const float ARTIFACT_MASS = 25f;

		[SerializeField] public Vector3 impactOffset;
		[SerializeField] public Vector2 target;
		[SerializeField] public bool aimAtTarget;
		[SerializeField] public float angleVariation;
		[SerializeField] public float chanceToBreak;

		public override void RandomizeVelocity()
		{
			if (aimAtTarget)
			{
				var dir = target - (Vector2)transform.position;
				var angle = -Vector3.Angle(Vector3.left, dir); // 0 is left

				spawnAngle = new Vector2(angle - angleVariation, angle + angleVariation);
			}

			base.RandomizeVelocity();
		}


		public new void Sim33ms(float dt)
		{
			velocity *= 1.05f;
			base.Sim33ms(dt);
		}

		public override void SpawnCraterPrefabs()
		{
			if (craterPrefabs != null && craterPrefabs.Length > 0)
			{
				var broken = Random.value < chanceToBreak;
				var prefab = Assets.GetPrefab(broken ? SimHashes.Ceramic.ToString() : craterPrefabs.GetRandom());

				if (broken && prefab.TryGetComponent(out PrimaryElement primaryElement))
					primaryElement.Mass = ARTIFACT_MASS;

				var position = transform.position with { z = -19.5f };

				var spawn = Util.KInstantiate(prefab, position);
				spawn.SetActive(true);

				if (broken)
					AudioUtil.PlaySound(ModAssets.Sounds.CERAMIC_BREAK, position, ModAssets.GetSFXVolume() * 2f);
			}
		}
	}
}
