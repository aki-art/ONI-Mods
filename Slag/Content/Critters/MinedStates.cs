namespace Slag.Content.Critters
{
    public class MinedStates : GameStateMachine<MinedStates, MinedStates.Instance, IStateMachineTarget, MinedStates.Def>
    {
        public State minePre;
        public State minepst;
        public State cowering;
        public State behaviourcomplete;

        public override void InitializeStates(out BaseState default_state)
        {
            default_state = cowering;

            cowering
                .Enter(smi => FUtility.Log.Debuglog("MINED STATES ENTERED"))
                .PlayAnim("hiding")
                .EventTransition((GameHashes)ModHashes.CritterMined, behaviourcomplete);

            behaviourcomplete
                .BehaviourComplete(ModAssets.Tags.grownShell);
        }

        public class Def : BaseDef
        {
        }

        public new class Instance : GameInstance
        {
            public Instance(Chore<Instance> chore, Def def) : base(chore, def)
            {
                chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, ModAssets.Tags.grownShell);
            }
        }
    }
}
