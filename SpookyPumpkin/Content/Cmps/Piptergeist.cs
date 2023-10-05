using UnityEngine;

namespace SpookyPumpkinSO.Content.Cmps
{
	public class Piptergeist : StateMachineComponent<Piptergeist.Instance>
	{
		public override void OnSpawn()
		{
			base.OnSpawn();
			smi.StartSM();
		}

		public class States : GameStateMachine<States, Instance, Piptergeist>
		{
			public State waitingForStorage;
			public State appear;
			public State rummaging;
			public State disappear;

			public TargetParameter storageTarget;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = waitingForStorage;

				waitingForStorage
					.PlayAnim("idle_loop", KAnim.PlayMode.Loop)
					.ParamTransition(storageTarget, appear, IsNotNull)
					.ScheduleGoTo(10f, disappear);

				appear
					.PlayAnim("rummage_pre")
					.OnAnimQueueComplete(rummaging);

				rummaging
					.PlayAnim("rummage_loop", KAnim.PlayMode.Loop)
					.Update(Rummage)
					.OnTargetLost(storageTarget, disappear)
					.ScheduleGoTo(100f, disappear);

				disappear
					.PlayAnim("rummage_pst")
					.QueueAnim("idle_loop", true)
					.Update(FadeOut, UpdateRate.SIM_33ms);
			}

			private void Rummage(Piptergeist.Instance smi, float _)
			{
				if (smi.targetStorage == null || smi.targetStorage.IsEmpty())
					return;

				if (Random.value <= 0.2f)
					return;

				var storage = smi.targetStorage;
				var item = smi.targetStorage.items.GetRandom();

				if (item.TryGetComponent(out Pickupable pickupable))
				{
					if (pickupable.UnreservedAmount > 0)
					{
						var go = storage.Drop(item);
						FUtility.Utils.YeetRandomly(go, true, 1, 3, false);
					}
				}
			}

			private void FadeOut(Piptergeist.Instance smi, float dt)
			{
				smi.elapsedTime += dt;

				if (smi.elapsedTime > Piptergeist.Instance.FADE_OUT_DURATION)
				{
					Destroy(smi);
					return;
				}

				smi.UpdateAlpha();
			}

			private static void Destroy(Piptergeist.Instance smi)
			{
				if (smi.targetStorage != null)
					smi.targetStorage.RemoveTag(GameTags.Creatures.ReservedByCreature);

				Util.KDestroyGameObject(smi.gameObject);
			}
		}

		public class Instance(Piptergeist master)
			: GameStateMachine<States, Instance, Piptergeist, object>.GameInstance(master)
		{
			public Storage targetStorage;
			public KBatchedAnimController kbac = master.GetComponent<KBatchedAnimController>();
			public float elapsedTime;
			public const float FADE_OUT_DURATION = 5f;
			public static Color transparent = new(1, 1, 1, 0);

			public void SetStorage(Storage storage)
			{
				targetStorage = storage;
				smi.sm.storageTarget.Set(storage, smi);
				storage.AddTag(GameTags.Creatures.ReservedByCreature);
			}

			public void UpdateAlpha()
			{
				var t = elapsedTime / FADE_OUT_DURATION;
				kbac.TintColour = Color.Lerp(Color.white, transparent, t);
			}
		}
	}
}
