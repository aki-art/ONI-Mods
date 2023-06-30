using Klei.AI;
using KSerialization;
using System.Runtime.Serialization;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class AETE_MinionStorage : KMonoBehaviour
	{
		[MyCmpReq] KBatchedAnimController kbac;
		[MyCmpReq] Effects effects;
		[MyCmpReq] MinionIdentity identity;
		[Serialize] private bool isDoubleTroubleDupe;

		public override void OnSpawn()
		{
			base.OnSpawn();

			if (effects.HasEffect(TEffects.DOUBLETROUBLE))
				kbac.TintColour = new Color(1, 1, 1, 0.5f);

			Subscribe((int)GameHashes.EffectRemoved, OnEffectRemoved);
		}

		private void OnEffectRemoved(object obj)
		{
			if (obj is Effect effect && effect.Id == TEffects.DOUBLETROUBLE)
				Die();
		}

		[OnDeserialized]
		private void OnDeserialized()
		{
			if (isDoubleTroubleDupe)
			{
				effects.Add(TEffects.DOUBLETROUBLE, true);
				isDoubleTroubleDupe = false;
			}
		}

		public void MakeItDouble()
		{
			kbac.TintColour = new Color(1, 1, 1, 0.5f);
			effects.Add(TEffects.DOUBLETROUBLE, true);
		}

		private void Die()
		{
			if (Game.IsQuitting())
				return;

			var equipment = identity.GetEquipment();
			if (equipment != null)
				equipment.UnequipAll();

			Game.Instance.SpawnFX(SpawnFXHashes.BuildingFreeze, transform.position, 0);
			Util.KDestroyGameObject(this);
		}
	}
}
