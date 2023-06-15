using FUtility;
using PrintingPodRecharge.Content.Items.BookI;

public class SelfImprovementChore : Chore<SelfImprovementChore.StatesInstance>
{
    public SelfImprovementChore(IStateMachineTarget book) : base(
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

    public class StatesInstance : GameStateMachine<States, StatesInstance, SelfImprovementChore, object>.GameInstance
    {
        public StatesInstance(SelfImprovementChore master) : base(master)
        {
        }
    }

    public class States : GameStateMachine<States, StatesInstance, SelfImprovementChore>
    {
        public TargetParameter reader;
        public TargetParameter readableSource;
        public TargetParameter bookResult;
        public FloatParameter requestedUnits;
        public FloatParameter actualUnits;
        public FetchSubState fetch;
        public State read;

        public override void InitializeStates(out BaseState default_state)
        {
            default_state = fetch;
            Target(reader);
            var state = fetch.InitializeStates(reader, readableSource, bookResult, requestedUnits, actualUnits, read);

            root
                .DoNothing();

            read
                .ToggleWork<SelfImprovementWorkable2>(bookResult, null, null, null);
        }
    }
}
