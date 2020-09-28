/*using System;

namespace InteriorDecorationVolI.Buildings.Aquarium
{
	public class FishTankStates : GameStateMachine<FishTankStates, FishTankStates.Instance>
	{
		public State empty;
		public State interact_refill;
		public State vacant;
		public State interact_delivery;
		public State occupied;
		public State dead;

		public BoolParameter IsDead;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = empty;

			empty
				.EventTransition(GameHashes.OnStorageChange, vacant, HasWater);
			vacant
				//.WorkableStartTransition(smi => smi.master.GetComponent<Storage>(), interact_delivery);
				.PlayAnim("filled")
				.Enter(smi => Debug.Log("has water"));
			//.EventTransition(GameHashes.OccupantChanged, interact_delivery, HasFish);

		}

		public static bool IsOperational(Instance smi) => smi.GetComponent<Operational>().IsOperational;

		private bool HasWater(Instance smi)
		{
			if (smi.GetComponent<FishTank>().waterStorage == null) return false;
			return smi.GetComponent<FishTank>().waterStorage.GetMassAvailable(SimHashes.Water) >= 100f;
		}
		public new class Instance : GameInstance
		{
			public Instance(IStateMachineTarget master) : base(master) { }
		}
	}
}
*/