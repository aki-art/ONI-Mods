
using STRINGS;
using System;
using UnityEngine;

namespace SpookyPumpkin
{
    class SeedTraderStates : GameStateMachine<SeedTraderStates, SeedTraderStates.Instance, IStateMachineTarget, SeedTraderStates.Def>
    {
        private const int MAX_NAVIGATE_DISTANCE = 100;
        public State idle;
        public State waiting;
        public State spitSeed;
        public State happy;
        public State behaviourcomplete;


        public override void InitializeStates(out BaseState default_state)
        {
            default_state = idle;
            root
                .ToggleStatusItem(CREATURES.STATUSITEMS.PLANTINGSEED.NAME, CREATURES.STATUSITEMS.PLANTINGSEED.TOOLTIP, category: Db.Get().StatusItemCategories.Main)
                .Exit(new StateMachine<SeedTraderStates, Instance, IStateMachineTarget, Def>.State.Callback(UnreserveSeed))
                .Exit(new StateMachine<SeedTraderStates, Instance, IStateMachineTarget, Def>.State.Callback(DropAll))
                .Exit(new StateMachine<SeedTraderStates, Instance, IStateMachineTarget, Def>.State.Callback(RemoveMouthOverride));

            idle
                .Enter(new StateMachine<SeedTraderStates, Instance, IStateMachineTarget, Def>.State.Callback(GivePumpkinSeed));
            behaviourcomplete
                .BehaviourComplete(GameTags.Creatures.WantsToPlantSeed);
        }

        private static void AddMouthOverride(Instance smi)
        {
            SymbolOverrideController component = smi.GetComponent<SymbolOverrideController>();
            KAnim.Build.Symbol symbol = smi.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol("sq_mouth_cheeks");
            if (symbol == null)
                return;
            component.AddSymbolOverride("sq_mouth", symbol);
        }

        private static void RemoveMouthOverride(Instance smi) => smi.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride("sq_mouth");

        private static void GivePumpkinSeed(Instance smi)
        {
            Debug.Log("given seed");
            var seedPrefab = Assets.GetPrefab(PumpkinPlantConfig.SEED_ID);
            smi.targetSeed = Util.KInstantiate(seedPrefab).GetComponent<Pickupable>();
            smi.targetSeed.TotalAmount = 1f;
            smi.targetSeed.gameObject.SetActive(true);

            UnreserveSeed(smi);
            smi.GetComponent<Storage>().Store(smi.targetSeed.gameObject);
            AddMouthOverride(smi);
        }


        private static void DropAll(SeedTraderStates.Instance smi) => smi.GetComponent<Storage>().DropAll();

        private static void ReserveSeed(SeedTraderStates.Instance smi)
        {
            GameObject go = smi.targetSeed ? smi.targetSeed.gameObject : null;
            if (!(go != null))
                return;
            DebugUtil.Assert(!go.HasTag(GameTags.Creatures.ReservedByCreature));
            go.AddTag(GameTags.Creatures.ReservedByCreature);
        }

        private static void UnreserveSeed(SeedTraderStates.Instance smi)
        {
            GameObject go = smi.targetSeed ? smi.targetSeed.gameObject : null;
            if (!(smi.targetSeed != null))
                return;
            go.RemoveTag(GameTags.Creatures.ReservedByCreature);
        }


        public class Def : BaseDef
        {
        }

        public new class Instance : GameInstance
        {
            public Pickupable targetSeed;

            public Instance(Chore<Instance> chore, Def def) : base(chore, def)
            {
                //chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToPlantSeed);
            }
        }
    }
}
