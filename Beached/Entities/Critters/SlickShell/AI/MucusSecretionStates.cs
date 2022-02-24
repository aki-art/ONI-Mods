using UnityEngine;

namespace Beached.Entities.Critters.SlickShell.AI
{
    public class MucusSecretionStates : GameStateMachine<MucusSecretionStates, MucusSecretionStates.Instance, IStateMachineTarget, MucusSecretionStates.Def>
    {
        public State secretePre;
        public State secretePost;
        public State behaviourComplete;

        public override void InitializeStates(out BaseState default_state)
        {
            default_state = secretePre;

            root
                .ToggleStatusItem(ModAssets.StatusItems.secretingMucus);

            secretePre
                .Enter(Secrete)
                .QueueAnim("lay_egg_pre")
                .OnAnimQueueComplete(secretePost);

            secretePost
                .QueueAnim("lay_egg_pst")
                .OnAnimQueueComplete(behaviourComplete);

            behaviourComplete
                .BehaviourComplete(ModAssets.Tags.Creatures.SecretingMucus);
        }

        private static void Secrete(Instance smi)
        {
            smi.position = smi.transform.GetPosition();
            smi.GetSMI<MoistureMonitor.Instance>().ProduceLubricant();
        }

        public class Def : BaseDef
        {
        }

        public new class Instance : GameInstance
        {
            public Vector3 position;

            public Instance(Chore<Instance> chore, Def def) : base(chore, def)
            {
                chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, ModAssets.Tags.Creatures.SecretingMucus);
            }
        }
    }
}
