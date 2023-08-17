using Klei;
using Klei.AI;
using STRINGS;
using System;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class CustomVomitChore : Chore<CustomVomitChore.StatesInstance>
	{
		public CustomVomitChore(
		  IStateMachineTarget target,
		  SimHashes element,
		  string vomitAnim,
		  Action<Chore> onComplete = null)
		  : base(Db.Get().ChoreTypes.Vomit, target, target.GetComponent<ChoreProvider>(), on_complete: onComplete, master_priority_class: PriorityScreen.PriorityClass.compulsory)
		{
			smi = new StatesInstance(this, element, vomitAnim, target.gameObject, Db.Get().DuplicantStatusItems.Vomiting);
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, CustomVomitChore, object>.GameInstance
		{
			public StatusItem statusItem;
			private AmountInstance bodyTemperature;
			private SafetyQuery vomitCellQuery;
			public SimHashes element;
			public string vomitAnim;
			public KBatchedAnimController kbac;

			public StatesInstance(CustomVomitChore master, SimHashes element, string vomitAnim, GameObject vomiter, StatusItem statusItem) : base(master)
			{
				this.element = element;
				this.vomitAnim = vomitAnim;
				sm.vomiter.Set(vomiter, smi, false);
				bodyTemperature = Db.Get().Amounts.Temperature.Lookup(vomiter);
				this.statusItem = statusItem;
				kbac = master.GetComponent<KBatchedAnimController>();
				vomitCellQuery = new SafetyQuery(Game.Instance.safetyConditions.VomitCellChecker, GetComponent<KMonoBehaviour>(), 10);
			}

			private static bool CanEmitLiquid(int cell) => !(Grid.Solid[cell] || (Grid.Properties[cell] & 2) != 0);

			public void SpawnElement(float dt)
			{
				if (dt <= 0)
					return;

				var totalTime = GetComponent<KBatchedAnimController>().CurrentAnim.totalTime;
				var num1 = dt / totalTime;
				var sicknesses = master.GetComponent<MinionModifiers>().sicknesses;
				var invalid = SimUtil.DiseaseInfo.Invalid;

				var idx = 0;

				while (idx < sicknesses.Count && sicknesses[idx].modifier.sicknessType != Sickness.SicknessType.Pathogen)
					++idx;

				var facinng = sm.vomiter.Get(smi).GetComponent<Facing>();
				var cell = Grid.PosToCell(facinng.transform.GetPosition());
				var frontCell = facinng.GetFrontCell();

				if (!CanEmitLiquid(frontCell))
					frontCell = cell;

				var equippable = GetComponent<SuitEquipper>().IsWearingAirtightSuit();

				if (equippable != null)
					equippable.GetComponent<Storage>()
						.AddLiquid(
						element,
						TUNING.STRESS.VOMIT_AMOUNT * num1,
						bodyTemperature.value,
						invalid.idx,
						invalid.count);
				else
					SimMessages.AddRemoveSubstance(
						frontCell,
						element,
						CellEventLogger.Instance.Vomit,
						TUNING.STRESS.VOMIT_AMOUNT * num1,
						bodyTemperature.value,
						invalid.idx,
						invalid.count);
			}

			public int GetVomitCell()
			{
				vomitCellQuery.Reset();

				Navigator component = GetComponent<Navigator>();
				component.RunQuery(vomitCellQuery);
				var vomitCell = vomitCellQuery.GetResultCell();

				if (Grid.InvalidCell == vomitCell)
					vomitCell = Grid.PosToCell(component);

				return vomitCell;
			}
		}

		public class States : GameStateMachine<States, StatesInstance, CustomVomitChore>
		{
			public TargetParameter vomiter;
			public State moveto;
			public VomitState vomit;
			public State recover;
			public State recover_pst;
			public State complete;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = moveto;

				Target(vomiter);

				root
					.ToggleAnims("anim_emotes_default_kanim");

				moveto
					.TriggerOnEnter(GameHashes.BeginWalk)
					.TriggerOnExit(GameHashes.EndWalk)
					.ToggleAnims("anim_loco_vomiter_kanim")
					.MoveTo((smi => smi.GetVomitCell()), vomit, vomit);

				vomit
					.DefaultState(vomit.buildup)
					.ToggleAnims(smi => smi.vomitAnim)
					.ToggleStatusItem(smi => smi.statusItem);

				vomit.buildup
					.PlayAnim("vomit_pre", KAnim.PlayMode.Once)
					.OnAnimQueueComplete(vomit.release);

				vomit.release
					.ToggleEffect("Vomiting")
					.PlayAnim("vomit_loop", KAnim.PlayMode.Once)
					.Update( (smi, dt) => smi.SpawnElement(dt))
					.OnAnimQueueComplete(vomit.release_pst);

				vomit.release_pst
					.PlayAnim("vomit_pst", KAnim.PlayMode.Once)
					.OnAnimQueueComplete(recover);

				recover
					.PlayAnim("breathe_pre")
					.QueueAnim("breathe_loop", true)
					.ScheduleGoTo(8f, recover_pst);

				recover_pst
					.QueueAnim("breathe_pst")
					.OnAnimQueueComplete(complete);

				complete
					.ReturnSuccess();
			}

			public class VomitState : State
			{
				public State buildup;
				public State release;
				public State release_pst;
			}
		}
	}
}
