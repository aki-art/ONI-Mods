using FUtility;
using PrintingPodRecharge.Items.BookI;

public class RidBadTraitsChore : Chore<RidBadTraitsChore.StatesInstance>
{
	public RidBadTraitsChore(IStateMachineTarget book) : base(
		Db.Get().ChoreTypes.GeneShuffle,
		book,
		null)
	{
		smi = new StatesInstance(this);
		smi.sm.readableSource.Set(book.gameObject, smi);
		smi.sm.requestedUnits.Set(1f, smi);

		showAvailabilityInHoverText = false;

		Prioritizable.AddRef(book.gameObject);

		Game.Instance.Trigger((int)GameHashes.UIRefresh, book.gameObject);

		AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, book.GetComponent<Assignable>());
		AddPrecondition(ChorePreconditions.instance.CanPickup, book.GetComponent<Pickupable>());
	}

	public override void Begin(Precondition.Context context)
	{
		if (context.consumerState.consumer == null || smi == null || smi.sm == null || smi.sm.readableSource == null)
		{
			Log.Error("GetRerolledChore null");
			return;
		}

		smi.sm.reader.Set(context.consumerState.gameObject, smi);
		base.Begin(context);
	}

	public class StatesInstance : GameStateMachine<States, StatesInstance, RidBadTraitsChore, object>.GameInstance
	{
		public StatesInstance(RidBadTraitsChore master) : base(master)
		{
		}
	}

	public class States : GameStateMachine<States, StatesInstance, RidBadTraitsChore>
	{
		public TargetParameter reader;
		public TargetParameter readableSource;
		public TargetParameter bookResult;
		public FloatParameter requestedUnits;
		public FloatParameter actualUnits;
		public FetchSubState fetch;
		public Read read;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = fetch;

			Target(reader);

			var state = fetch.InitializeStates(
				reader,
				readableSource,
				bookResult,
				requestedUnits,
				actualUnits,
				read);

			root
				.DoNothing();

			read
				.ToggleWork<SelfImprovementWorkable>(bookResult, null, null, null);
		}

		public class Read : State
		{
			public State pre;
			public State pst;
		}
	}
}