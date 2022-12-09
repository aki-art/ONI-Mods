﻿using Klei.AI;
using System;
using TUNING;

namespace MayISit.Content.Scripts
{
	public class SitChore : Chore<SitChore.StatesInstance>, IWorkerPrioritizable
	{
		public static Precondition IsIdle;

		public int basePriority = RELAXATION.PRIORITY.TIER0;
		public string specificEffect = ModDb.Effects.LOUNGED;
		public string trackingEffect = ModDb.Effects.RECENTLY_LOUNGED;

		public SitChore(IStateMachineTarget master, Action<Chore> on_end = null) 
			: base(
                  Db.Get().ChoreTypes.Relax,
                  master,
                  master.GetComponent<ChoreProvider>(),
                  true,
                  null,
                  null,
                  on_end,
                  PriorityScreen.PriorityClass.high,
                  5,
                  false,
                  true,
                  0,
                  false,
                  ReportManager.ReportType.PersonalTime)
		{
			smi = new StatesInstance(this);
			//base.AddPrecondition(ChorePreconditions.instance.CanMoveTo, chat_workable);
			AddPrecondition(ChorePreconditions.instance.IsNotRedAlert);
			AddPrecondition(ChorePreconditions.instance.IsScheduledTime, Db.Get().ScheduleBlockTypes.Recreation);
			AddPrecondition(ChorePreconditions.instance.CanDoWorkerPrioritizable, this);
		}


        public override void Begin(Precondition.Context context)
		{
			smi.sm.sitter.Set(context.consumerState.gameObject, smi);
			base.Begin(context);
		}

		public bool GetWorkerPriority(Worker worker, out int priority)
		{
			priority = basePriority;

			var effect = worker.GetComponent<Effects>();

			if (!string.IsNullOrEmpty(trackingEffect) && effect.HasEffect(trackingEffect))
			{
				priority = 0;
				return false;
			}

			if (!string.IsNullOrEmpty(specificEffect) && effect.HasEffect(specificEffect))
			{
				priority = RELAXATION.PRIORITY.RECENTLY_USED;
			}

			return true;
		}

        public class States : GameStateMachine<States, StatesInstance, SitChore>
		{
			public TargetParameter sitter;

			public ApproachSubState<Seat> moveToSeat;
			public SitStates sit;
			public State chat;
			public State success;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = moveToSeat;

				Target(sitter);

				moveToSeat
					.InitializeStates(sitter, masterTarget, sit);

				sit
					//.ToggleAnims("anim_interacts_watercooler_kanim")
					.ToggleAnims("anim_interacts_sit_kanim")
					.DefaultState(sit.sitting);

				sit.sitting
					//.Face(masterTarget, 0.5f)
					.PlayAnim("sit_pre", KAnim.PlayMode.Once)
					.QueueAnim("sit_loop", true)
					//.Transition(sit.done, DoneSitting, UpdateRate.SIM_1000ms)
					.ScheduleGoTo(30f, sit.done); // temporary
					//.OnAnimQueueComplete(sit.done);

				sit.done
					.Enter("Drink", GetUp)
					.PlayAnim("working_pst")
					.OnAnimQueueComplete(success);

				success
					.ReturnSuccess();
			}

            private bool DoneSitting(StatesInstance smi)
            {
				return false;
            }

			private void GetUp(StatesInstance smi)
			{
				var effects = stateTarget.Get<Worker>(smi).GetComponent<Effects>();

				AddEffect(smi.master.trackingEffect, effects);
				//AddEffect(smi.master.specificEffect, effects);
			}

			private void AddEffect(string effect, Effects effects)
            {
				if(!string.IsNullOrEmpty(effect))
				{
					effects.Add(effect, true);
				}
			}

			public class SitStates : State
			{
				public State sitting;
				public State done;
			}
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, SitChore, object>.GameInstance
		{
			public StatesInstance(SitChore master) : base(master)
			{
			}
		}
	}
}