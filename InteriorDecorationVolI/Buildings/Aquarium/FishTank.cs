using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InteriorDecorationVolI.Buildings.Aquarium
{
    class FishTank : StateMachineComponent<FishTank.SMInstance>
	{
		private int HP = 100;
		private List<FetchOrder2> fetches;
		public CellOffset[] deliveryOffsets = new CellOffset[1];
		public CellOffset spawnOffset = new CellOffset(0, 0);

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			fetches = new List<FetchOrder2>();
		}
		protected override void OnSpawn()
		{
			base.OnSpawn();
			smi.StartSM();
		}
		public class States : GameStateMachine<States, SMInstance, FishTank>
		{
#pragma warning disable 0649
			public State empty;
			public State interact_refill;
			public State vacant;
			public State interact_delivery;
			public State occupied;
			public State dead;
			public DyingStates dying;

			public BoolParameter IsOverHeating;
			public BoolParameter IsFreezing;
			public BoolParameter IsBoiling;
			public BoolParameter IsBuried;
			public BoolParameter IsDead;

			public class DyingStates : State
			{
				public State overheating;
				public State boiling;
				public State frozen;
				public State buried;
			}
#pragma warning restore 0649

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = empty;

				empty
					.EventTransition(GameHashes.OnStorageChange, vacant, HasWater);
				vacant
					//.WorkableStartTransition(smi => smi.master.GetComponent<Storage>(), interact_delivery);
					.EventTransition(GameHashes.OccupantChanged, interact_delivery, HasFish);
				interact_delivery
					.GoTo(occupied);
				occupied
					.EventTransition(GameHashes.OccupantChanged, vacant, Not(HasFish))
					.ParamTransition(IsOverHeating, dying.overheating, IsTrue)
					.ParamTransition(IsFreezing, dying.frozen, IsTrue)
					.ParamTransition(IsBoiling, dying.boiling, IsTrue)
					.ParamTransition(IsBuried, dying.buried, IsTrue)
					.Update(CheckDying, UpdateRate.RENDER_1000ms);
				dying.DefaultState(dying.frozen);
				dying.root
					.Update(CheckDying, UpdateRate.RENDER_1000ms)
					.ParamTransition(IsDead, dead, IsTrue);
				dying.frozen
					.ParamTransition(IsFreezing, dying.buried, IsFalse);
				dying.buried
					.ParamTransition(IsBuried, dying.overheating, IsFalse);
				dying.overheating
					.ParamTransition(IsOverHeating, dying.boiling, IsFalse);
				dying.boiling
					.ParamTransition(IsBoiling, occupied, IsFalse);
				dead
					.EnterTransition(empty, Not(HasWater))
					.GoTo(vacant);


			}

			private bool HasFish(SMInstance smi)
			{
				throw new NotImplementedException();
			}

			private void CheckDying(SMInstance arg1, float arg2)
			{
				throw new NotImplementedException();
				// check if it is dying
				// deduct HP
			}

			private bool HasWater(SMInstance smi)
			{
				throw new NotImplementedException();
			}
		}

		public class SMInstance : GameStateMachine<States, SMInstance, FishTank, object>.GameInstance
		{
			public SMInstance(FishTank master) : base(master) { }
		}
	}
}
