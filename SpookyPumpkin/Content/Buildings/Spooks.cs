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
						smi.GetComponent<Light2D>().Color = orange;
					})
					.EventTransition(GameHashes.OperationalChanged, off, smi => !smi.operational.IsOperational);

				spooked
					.PlayAnim("spook", KAnim.PlayMode.Once)
					.Enter(smi => smi.GetComponent<Light2D>().Color = green)
					.EventTransition(GameHashes.OperationalChanged, off, smi => !smi.operational.IsOperational)
					.ScheduleGoTo(3f, on);
			}
		}

		public class SMInstance : GameStateMachine<States, SMInstance, Spooks, object>.GameInstance
		{
			private EmoteReactable reactable;
			public Operational operational;

			public SMInstance(Spooks master) : base(master)
			{
				operational = master.GetComponent<Operational>();
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
				var kbac = reactor.GetComponent<KBatchedAnimController>();
				kbac.Queue("fall_pre");
				kbac.Queue("fall_loop");
				kbac.Queue("fall_loop");
				kbac.Queue("fall_pst");

				PlaySound3DAtLocation(GlobalAssets.GetSound("dupvoc_03_voice_wailing"), transform.position);
				PlaySound3DAtLocation(GlobalAssets.GetSound("dupvoc_02_voice_destructive_enraging"), transform.position);

				reactor.GetComponent<Effects>().Add(SPEffects.SPOOKED, true);

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