namespace DuctTape.Components
{
    public class StorageDistributor : GameStateMachine<StorageDistributor, StorageDistributor.Instance, IStateMachineTarget, StorageDistributor.Def>
    {
        private State idle;

        public override void InitializeStates(out BaseState default_state)
        {
            default_state = idle;
        }

        public class Def : BaseDef
        {
        }

        public new class Instance : GameInstance
        {
            public Instance(IStateMachineTarget master, Def def) : base(master, def)
            {
            }
        }
    }
}
