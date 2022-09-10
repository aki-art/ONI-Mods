using FUtility;
using PrintingPodRecharge.Items;

public class GetRerolledChore : Chore<GetRerolledChore.StatesInstance>
{
	public GetRerolledChore(IStateMachineTarget shaker) : base(
        Db.Get().ChoreTypes.GeneShuffle,
        shaker,
        null)
	{
        smi = new StatesInstance(this);
        smi.sm.equippableSource.Set(shaker.gameObject, smi);
        smi.sm.requestedUnits.Set(1f, smi);

		showAvailabilityInHoverText = false;

		Prioritizable.AddRef(shaker.gameObject);

		Game.Instance.Trigger((int)GameHashes.UIRefresh, shaker.gameObject);

        AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, shaker.GetComponent<Assignable>());
        AddPrecondition(ChorePreconditions.instance.CanPickup, shaker.GetComponent<Pickupable>());
	}

	public override void Begin(Precondition.Context context)
	{
		if (context.consumerState.consumer == null || smi == null || smi.sm == null || smi.sm.equippableSource == null)
		{
			Log.Error("GetRerolledChore null");
			return;
		}

        smi.sm.equipper.Set(context.consumerState.gameObject, smi);
		base.Begin(context);
	}

	public class StatesInstance : GameStateMachine<States, StatesInstance, GetRerolledChore, object>.GameInstance
	{
		public StatesInstance(GetRerolledChore master) : base(master)
		{
		}
	}

	public class States : GameStateMachine<States, StatesInstance, GetRerolledChore>
	{
		public TargetParameter equipper;
		public TargetParameter equippableSource;
		public TargetParameter shakerResult;
		public FloatParameter requestedUnits;
		public FloatParameter actualUnits;
		public FetchSubState fetch;
		public Equip equip;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = fetch;

			Target(equipper);
			root.DoNothing();

			var state = fetch.InitializeStates(
                equipper,
                equippableSource,
                shakerResult,
                requestedUnits,
                actualUnits,
                equip);

			equip.ToggleWork<ShakerWorkable>(shakerResult, null, null, null);
		}

		public class Equip : State
		{
			public State pre;
			public State pst;
		}
	}
}
