using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class AETE_DuplicatedDupe : KMonoBehaviour, ISim1000ms
	{
		public float remainingLifeTimeSeconds;

		[MyCmpReq] KBatchedAnimController kbac;

		public override void OnSpawn()
		{
			base.OnSpawn();
			if (TryGetComponent(out KSelectable kSelectable))
				kSelectable.AddStatusItem(TStatusItems.DupeStatus, this);

			kbac.TintColour = new Color(1, 1, 1, 0.5f);
		}

		public void Sim1000ms(float dt)
		{
			remainingLifeTimeSeconds -= dt;
			if (remainingLifeTimeSeconds <= 0)
				Die();
		}

		public object GetDeathTime() => GameUtil.GetFormattedTime(remainingLifeTimeSeconds);

		private void Die()
		{
			Game.Instance.SpawnFX(SpawnFXHashes.BuildingFreeze, transform.position, 0);
			Util.KDestroyGameObject(this);
		}
	}
}
