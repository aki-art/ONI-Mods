using FUtility;
using Klei.AI;
using UnityEngine;

namespace Beached.Entities.Critters.SlickShell.AI
{
    public class MoistureMonitor : GameStateMachine<MoistureMonitor, MoistureMonitor.Instance, IStateMachineTarget, MoistureMonitor.Def>
    {
        private State wet;
        private State desiccating;
        private DryStates dry;

        public override void InitializeStates(out BaseState default_state)
        {
            default_state = wet;

            wet
                .Enter(Moisturize)
                .Transition(dry.damp, Not(IsInLiquid));

            dry
                .DefaultState(dry.damp)
                .ToggleStateMachine(smi => new LubricatedMovementMonitor.Instance(smi.master))
                .Enter(ApplyModifier)
                .Enter(smi => SetSpeedModifier(smi, 0.66f))
                .Transition(wet, IsInLiquid)
                .Transition(desiccating, IsCompletelyDry)
                .Exit(RemoveModifier);

            dry.damp
                .UpdateTransition(dry.dry, UpdateDrying);

            dry.dry
                .ToggleBehaviour(ModAssets.Tags.Creatures.SecretingMucus, CanProduceLubricant);

            desiccating
                .Enter(smi => SetSpeedModifier(smi, 0.33f))
                .ToggleStatusItem(ModAssets.StatusItems.desiccation)
                .Transition(wet, IsInLiquid);
        }

        private bool UpdateDrying(Instance smi, float dt)
        {
            smi.hasBeenDryFor += dt;
            return smi.hasBeenDryFor >= 10f && smi.moisture.value < 80f;
        }

        private bool CanProduceLubricant(Instance smi)
        {
            int cell = Grid.CellBelow(Grid.PosToCell(smi));
            return Grid.IsValidCell(cell) && Grid.IsSolidCell(cell);
        }

        public class DryStates : State
        {
            public State damp;
            public State dry;
        }

        private void SetSpeedModifier(Instance smi, float amount)
        {
            smi.navigator.defaultSpeed = smi.originalSpeed * amount;
        }

        private void Moisturize(Instance smi)
        {
            smi.moisture.SetValue(100f);
            smi.navigator.defaultSpeed = smi.originalSpeed;
        }

        private static void ApplyModifier(Instance smi)
        {
            smi.moisture.deltaAttribute.Add(smi.baseMoistureModifier);
        }

        private static void RemoveModifier(Instance smi)
        {
            smi.moisture.deltaAttribute.Remove(smi.baseMoistureModifier);
        }

        private static bool IsCompletelyDry(Instance smi)
        {
            return smi.moisture.value <= 0;
        }

        private static bool IsInLiquid(Instance smi)
        {
            var cell = Grid.PosToCell(smi);
            return Grid.Element[cell].IsLiquid;
        }

        public class Def : BaseDef//, IGameObjectEffectDescriptor
        {
            internal float defaultDryRate = -30f / Consts.CYCLE_LENGTH;
            public SimHashes lubricant;
            public float lubricantMassKg;
            public float lubricantTemperatureKelvin;

            public override void Configure(GameObject prefab)
            {
                prefab.GetComponent<Modifiers>().initialAmounts.Add(Amounts.Moisture.Id);
            }
        }

        public new class Instance : GameInstance
        {
            public AmountInstance moisture;
            public AttributeModifier baseMoistureModifier;
            public float originalSpeed;
            public Navigator navigator;
            public float hasBeenDryFor;

            public Instance(IStateMachineTarget master, Def def) : base(master, def)
            {
                moisture = Amounts.Moisture.Lookup(gameObject);
                moisture.value = moisture.GetMax();

                baseMoistureModifier = new AttributeModifier(
                    moisture.amount.deltaAttribute.Id,
                    def.defaultDryRate,
                    STRINGS.CREATURES.MODIFIERS.MOISTURE_LOSS_RATE.NAME,
                    false,
                    false,
                    true);

                navigator = smi.GetComponent<Navigator>();
                originalSpeed = navigator.defaultSpeed;
            }

            public void ProduceLubricant()
            {
                 BubbleManager.instance.SpawnBubble(
                     transform.GetPosition(), 
                     Vector2.zero, 
                     def.lubricant, 
                     def.lubricantMassKg, 
                     def.lubricantTemperatureKelvin);
            }
        }
    }
}
