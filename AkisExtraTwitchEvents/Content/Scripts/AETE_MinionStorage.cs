using KSerialization;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class AETE_MinionStorage : KMonoBehaviour, ISim1000ms
	{
		public float remainingLifeTimeSeconds;

		[MyCmpReq] KBatchedAnimController kbac;
		[Serialize] private bool isDoubleTroubleDupe;

		public override void OnSpawn()
		{
			base.OnSpawn();
			if (isDoubleTroubleDupe)
				MakeItDouble();
		}

		public void MakeItDouble()
		{
			if (TryGetComponent(out KSelectable kSelectable))
				kSelectable.AddStatusItem(TStatusItems.DupeStatus, this);

			kbac.TintColour = new Color(1, 1, 1, 0.5f);
			isDoubleTroubleDupe = true;
		}

		public void Sim1000ms(float dt)
		{
			if (isDoubleTroubleDupe)
			{
				remainingLifeTimeSeconds -= dt;
				if (remainingLifeTimeSeconds <= 0)
					Die();
			}
		}

		public object GetDeathTime() => GameUtil.GetFormattedTime(remainingLifeTimeSeconds);

		private void Die()
		{
			Game.Instance.SpawnFX(SpawnFXHashes.BuildingFreeze, transform.position, 0);
			Util.KDestroyGameObject(this);
		}
	}
}
