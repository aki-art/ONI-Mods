using Klei.AI;
using System;

namespace Beached.Components
{
    // Makes something periodically duplicate itself
    public class SelfDuplicatorPlant : StateMachineComponent<SelfDuplicatorPlant.SMInstance> //, IGameObjectEffectDescriptor
    {
        private AmountInstance maturity;

        [MyCmpGet]
        Growing growing;

        protected override void OnPrefabInit()
        {
            Amounts amounts = gameObject.GetAmounts();
            maturity = amounts.Get(Db.Get().Amounts.Maturity);

            Subscribe((int)GameHashes.NewGameSpawn, OnNewGameSpawn);;
        }

        private void OnNewGameSpawn(object data)
        {

        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            base.smi.StartSM();
            // base.gameObject.AddTag(GameTags.GrowingPlant);
        }

        public class States : GameStateMachine<States, SMInstance, SelfDuplicatorPlant>
        {
            State idle;
            State duplicating;
            State done;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = idle;

                idle
                    .EventTransition(GameHashes.Grow, duplicating, IsPlantGrown);

                duplicating
                    .EventTransition(GameHashes.Wilt, idle)
                    .EventTransition(GameHashes.Uprooted, done)
                    .UpdateTransition(done, Grow, UpdateRate.SIM_4000ms);

                done
                    .DoNothing();
            }

            private bool IsPlantGrown(SMInstance smi)
            {
                return smi.growing.IsGrown();
            }

            public bool CanGrowInto(int cell)
            {
                return Grid.IsValidCell(cell) &&
                    !Grid.Solid[cell] &&
                    !Grid.IsSubstantialLiquid(cell, 0.35f) && 
                    (Grid.Objects[cell, (int)ObjectLayer.Building] is null) && 
                    (Grid.Objects[cell, (int)ObjectLayer.Plants] is null) && 
                    !Grid.Foundation[cell];
            }

            private void Restart(SMInstance smi)
            {
                smi.GoTo(duplicating);
            }

            private bool Grow(SMInstance smi, float dt)
            {
                int pos = Grid.CellAbove(Grid.PosToCell(smi));

                if (CanGrowInto(pos))
                {
                    var child = FUtility.Utils.Spawn(smi.PrefabID(), Grid.CellToPos(pos));
                    if(child != null)
                    {
                        child.Subscribe((int)GameHashes.Uprooted, obj => Restart(smi));
                    }
                }

                return false;
            }
        }

        public class SMInstance : GameStateMachine<States, SMInstance, SelfDuplicatorPlant, object>.GameInstance
        {
            public Growing growing;

            public SMInstance(SelfDuplicatorPlant master) : base(master)
            {
                growing = master.GetComponent<Growing>();  
            }
        }
    }
}
