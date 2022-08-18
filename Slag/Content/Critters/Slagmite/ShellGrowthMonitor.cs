using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

namespace Slag.Content.Critters.Slagmite
{
    public class ShellGrowthMonitor : GameStateMachine<ShellGrowthMonitor, ShellGrowthMonitor.Instance, IStateMachineTarget, ShellGrowthMonitor.Def>
    {
        public State growing;
        public State grown;

        public override void InitializeStates(out BaseState default_state)
        {
            default_state = growing;

            growing
                .ScheduleGoTo(10f, grown);

            grown
                .ToggleStatusItem("GROWN", "")
                .ToggleTag(ModAssets.Tags.grownShell)
                .ToggleBehaviour(ModAssets.Tags.grownShell, smi => true);
        }

        public class Def : BaseDef, IGameObjectEffectDescriptor
        {
            public int levelCount;
            public float defaultGrowthRate;
            public SimHashes targetAtmosphere;

            public List<Descriptor> GetDescriptors(GameObject go)
            {
                return new List<Descriptor>();
            }

            public override void Configure(GameObject prefab)
            {
                prefab.GetComponent<Modifiers>().initialAmounts.Add(SAmounts.ShellGrowth.Id);
            }
        }

        public new class Instance : GameInstance
        {
            public AmountInstance shellGrowth;
            public AttributeModifier growingGrowthModifier;
            public AttributeModifier stuntedGrowthModifier;

            public int currentScaleLevel = -1;

            public bool IsFullyGrown()
            {
                return currentScaleLevel == def.levelCount;
            }

            public void Mine()
            {
                currentScaleLevel = 0;
            }

            public Instance(IStateMachineTarget master, Def def) : base(master, def)
            {
                shellGrowth = SAmounts.ShellGrowth.Lookup(gameObject);
                shellGrowth.value = shellGrowth.GetMax();

                //growingGrowthModifier = new AttributeModifier(shellGrowth.amount.deltaAttribute.Id, def.defaultGrowthRate * 100f, STRINGS.CREATURES.MODIFIERS.ELEMENT_GROWTH_RATE.NAME);
                //stuntedGrowthModifier = new AttributeModifier(shellGrowth.amount.deltaAttribute.Id, def.defaultGrowthRate * 20f, STRINGS.CREATURES.MODIFIERS.ELEMENT_GROWTH_RATE.NAME);
            }
        }

    }
}
