using FUtility;

namespace Slag.Content.Critters.Slagmite
{
    public class MinedStates : GameStateMachine<MinedStates, MinedStates.Instance, IStateMachineTarget, MinedStates.Def>
    {
        public State cowering;
        public State behaviourcomplete;

        public override void InitializeStates(out BaseState default_state)
        {
            default_state = cowering;

            cowering
                .DebugStatusItem("MinedStates - cowering")
                .PlayAnim("hiding")
                .EventTransition((GameHashes)ModHashes.CritterMined, behaviourcomplete)
                .TagTransition(ModAssets.Tags.beingMined, behaviourcomplete, true);

            behaviourcomplete
                .DebugStatusItem("MinedStates - behaviourcomplete")
                .BehaviourComplete(ModAssets.Tags.beingMined);
        }

        public class Def : BaseDef
        {
        }

        public new class Instance : GameInstance
        {
            public Instance(Chore<Instance> chore, Def def) : base(chore, def)
            {
                chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, ModAssets.Tags.beingMined);
            }
        }
    }
}
