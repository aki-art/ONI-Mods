using FUtility;
using KSerialization;
using System;

namespace Twitchery.Content.Scripts
{
	// TODO: hat
	// TODO: balloon

	[SerializationConfig(MemberSerialization.OptIn)]
	public class AETE_PolymorphCritter : KMonoBehaviour, ISim200ms
	{
		[MyCmpReq] private KSelectable kSelectable;
		[MyCmpReq] private MinionStorage minionStorage;
		[MyCmpReq] private KBatchedAnimController kbac;

		[Serialize] public float duration;
		[Serialize] public float elapsedTime;
		[Serialize] public string originalSpeciesname;

		public override void OnSpawn()
		{
			base.OnSpawn();
			kSelectable.AddStatusItem(TStatusItems.PolymorphStatus, this);
			NameDisplayScreen.Instance.RegisterComponent(gameObject, this);
		}

		public override void OnCleanUp()
		{
			ReleaseMinions();
			base.OnCleanUp();
		}

		private void ReleaseMinions()
		{
			if (minionStorage.serializedMinions == null)
			{
				Log.Warning("No stored minion in AETE_PolymorphCritter");
				return;
			}

			// there should be only one, but just in case
			for (var i = 0; i < minionStorage.serializedMinions.Count; i++)
			{
				var minion = minionStorage.serializedMinions[i];
				minionStorage.DeserializeMinion(minion.id, transform.position);
			}
		}

		public void Sim200ms(float dt)
		{
			elapsedTime += dt;

			if (elapsedTime >= duration)
				Util.KDestroyGameObject(gameObject);
		}

		public void SetMorph(MinionIdentity identity, Polymorph morph)
		{
			UpdateName(identity);

			duration = 100f;
			originalSpeciesname = morph.Name;
			kbac.SwapAnims(new[] { Assets.GetAnim(morph.animFile) });

			UpdateBalloon(identity);

			minionStorage.SerializeMinion(identity.gameObject);
		}

		private void UpdateBalloon(MinionIdentity identity)
		{
			if (identity.TryGetComponent(out Equipment equipment))
			{
				var toySlot = equipment.GetSlot(Db.Get().AssignableSlots.Toy);
				if (toySlot == null || toySlot.assignable)
					return;

				if (toySlot.assignable.IsPrefabID(EquippableBalloonConfig.ID))
				{

				}
			}
		}

		private void UpdateName(MinionIdentity identity)
		{
			var name = identity.GetProperName();
			kSelectable.SetName(name);
			NameDisplayScreen.Instance.UpdateName(gameObject);
			Trigger((int)GameHashes.NameChanged, name);
		}
	}
}
