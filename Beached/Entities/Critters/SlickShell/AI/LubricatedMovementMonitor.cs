using FUtility;
using Klei.AI;

namespace Beached.Entities.Critters.SlickShell.AI
{
    internal class LubricatedMovementMonitor : GameStateMachine<LubricatedMovementMonitor, LubricatedMovementMonitor.Instance, IStateMachineTarget, LubricatedMovementMonitor.Def>
    {
        public State idle;
        public State moving;

        public override void InitializeStates(out BaseState default_state)
        {
            default_state = idle;

            idle
                .EnterTransition(moving, smi => smi.GetComponent<Navigator>().IsMoving())
                .EventHandlerTransition(GameHashes.ObjectMovementStateChanged, moving, IsMoving);

            moving
                .Enter(ApplyModifier)
                .EventHandlerTransition(GameHashes.ObjectMovementStateChanged, idle, (smi, data) => !IsMoving(smi, data))
                .Exit(RemoveModifier);
        }

        private bool IsMoving(Instance smi, object data)
        {
            return data is GameHashes hash && hash == GameHashes.ObjectMovementWakeUp;
        }

        private static void ApplyModifier(Instance smi)
        {
            smi.moisture.deltaAttribute.Add(smi.movementMoistureModifier);
        }

        private static void RemoveModifier(Instance smi)
        {
            smi.moisture.deltaAttribute.Remove(smi.movementMoistureModifier);
        }

        public class Def : BaseDef
        {
        }

        public new class Instance : GameInstance
        {
            public AmountInstance moisture;
            public AttributeModifier movementMoistureModifier;

            internal float movingDryRate = -500f / Consts.CYCLE_LENGTH;


            public Instance(IStateMachineTarget master) : base(master)
            {
                moisture = Amounts.Moisture.Lookup(gameObject);

                movementMoistureModifier = new AttributeModifier(
                    moisture.amount.deltaAttribute.Id,
                    movingDryRate,
                    STRINGS.CREATURES.MODIFIERS.MOVEMENT_MOISTURE_LOSS.NAME,
                    false,
                    false,
                    true);
            }
        }
    }
}
