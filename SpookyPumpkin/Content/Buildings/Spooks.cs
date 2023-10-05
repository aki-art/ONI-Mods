using Klei.AI;
using UnityEngine;

namespace SpookyPumpkinSO.Content.Buildings
{
	public class Spooks : StateMachineComponent<Spooks.SMInstance>
	{
		public override void OnSpawn()
		{
			base.OnSpawn();
			smi.StartSM();
		}

		public class States : GameStateMachine<States, SMInstance, Spooks>
		{
			public State off;
			public State on;
			public State spooked;

			private Color orange = new Color(2, 1.5f, 0.3f, 2f);
			private Color green = new Color(0.3f, 3f, 1f, 2f);

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = off;

				off
					.Enter(smi => smi.ClearReactable())
					.PlayAnim("off")
					.EventTransition(GameHashes.OperationalChanged, on, smi => smi.operational.IsOperational);

				on
					.PlayAnim("on")
					.Enter(smi =>
					{
						smi.CreateReactable();
						smi.light2D.Color = orange;
					})
					.EventTransition(GameHashes.OperationalChanged, off, smi => !smi.operational.IsOperational);

				spooked
					.PlayAnim("spook", KAnim.PlayMode.Once)
					.Enter(smi => smi.light2D.Color = green)
					.EventTransition(GameHashes.OperationalChanged, off, smi => !smi.operational.IsOperational)
					.ScheduleGoTo(3f, on);
			}
		}

		public class SMInstance : GameStateMachine<States, SMInstance, Spooks, object>.GameInstance
		{
			private EmoteReactable reactable;
			public Operational operational;
			public Light2D light2D;

			public SMInstance(Spooks master) : base(master)
			{
				operational = master.GetComponent<Operational>();
				light2D = master.GetComponent<Light2D>();
			}

			public void CreateReactable()
			{
				if (reactable == null)
				{
					reactable = new EmoteReactable(gameObject, "SP_Spooked", Db.Get().ChoreTypes.Emote, 3, 2, 0, 120)
					.SetEmote(Db.Get().Emotes.Minion.Shock);

					reactable.RegisterEmoteStepCallbacks("react", Spook, null);
				}
			}

			private void Spook(GameObject reactor)
			{
				if (reactor == null || reactor.transform == null)
					return;

				if (reactor.TryGetComponent(out KBatchedAnimController kbac))
				{
					kbac.Queue("fall_pre");
					kbac.Queue("fall_loop");
					kbac.Queue("fall_loop");
					kbac.Queue("fall_pst");
				}

				var pos = reactor.transform.position;
				PlaySound3DAtLocation(GlobalAssets.GetSound("dupvoc_03_voice_wailing"), pos);
				PlaySound3DAtLocation(GlobalAssets.GetSound("dupvoc_02_voice_destructive_enraging"), pos);

				if (reactor.TryGetComponent(out Effects effects))
					effects.Add(SPEffects.SPOOKED, true);

				if (smi != null)
					smi.GoTo(smi.sm.spooked);
			}

			public void ClearReactable()
			{
				if (reactable != null)
				{
					reactable.Cleanup();
					reactable = null;
				}
			}
		}
	}
}