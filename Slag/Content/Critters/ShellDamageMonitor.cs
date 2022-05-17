using FUtility;
using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

namespace Slag.Content.Critters
{
    public class ShellDamageMonitor : GameStateMachine<ShellDamageMonitor, ShellDamageMonitor.Instance, IStateMachineTarget, ShellDamageMonitor.Def>
    {
        protected static HashedString[] symbols = new HashedString[]
        {
            "shell_breaking_0",
            "shell_breaking_1",
            "shell_breaking_2",
            "shell_breaking_3",
            "shell_breaking_4",
            "shell_breaking_5",
            "shell_breaking_6",
            "shell_breaking_7",
            "shell_breaking_8",
            "shell_breaking_9",
            "shell_breaking_10",
            "shell_breaking_11",
            "shell_breaking_12"
        };

        protected static int symbolCount = symbols.Length;

        public override void InitializeStates(out BaseState default_state)
        {
            default_state = root;
        }

        public class Def : BaseDef//, IGameObjectEffectDescriptor
        {
            public Tag itemDroppedOnShear;
            public float dropMass;

            public List<Descriptor> GetDescriptors(GameObject go)
            {
                return new List<Descriptor>();
            }

            public override void Configure(GameObject prefab)
            {
                //prefab.GetComponent<Modifiers>().initialAmounts.Add(ModAssets.Amounts.ShellGrowth.Id);
            }
        }

        public new class Instance : GameInstance
        {
            public float health = 1f;
            public AmountInstance shellGrowth;
            private KBatchedAnimController kbac;

            public bool IsBroken()
            {
                return health <= 0f;
            }

            private void RefreshSymbols()
            {
                var symbolIdx = health >= 1f ? 0 : Mathf.FloorToInt(symbols.Length * (1f - health));
                for (var i = 0; i < symbols.Length; i++)
                {
                    kbac.SetSymbolVisiblity(symbols[i], symbolIdx == i);
                }
            }

            public void Restore()
            {
                health = 1f;
                RefreshSymbols();
            }

            public void Damage(float amount)
            {
                if (shellGrowth.value < shellGrowth.GetMax())
                {
                    return;
                }

                health -= amount;


                if (health <= 0f)
                {
                    Break();
                    return;
                }

                RefreshSymbols();
            }

            public void Break()
            {
                if (shellGrowth.value < shellGrowth.GetMax())
                {
                    return;
                }

                SpawnProduct();
                master.gameObject.GetSMI<ShellGrowthMonitor.Instance>().Mine();
                Restore();
            }

            private void SpawnProduct()
            {
                var result = Util.KInstantiate(Assets.GetPrefab(def.itemDroppedOnShear), null, null);

                var othersPrimaryElement = smi.GetComponent<PrimaryElement>();

                var primaryElement = result.GetComponent<PrimaryElement>();
                primaryElement.Temperature = othersPrimaryElement.Temperature;
                primaryElement.Mass = def.dropMass;
                primaryElement.AddDisease(othersPrimaryElement.DiseaseIdx, othersPrimaryElement.DiseaseCount, "Mined");

                result.SetActive(true);

                result.transform.position = transform.GetPosition();

                Utils.YeetRandomly(result, true, 0.3f, 1.5f, false);
            }

            public Instance(IStateMachineTarget master, Def def) : base(master, def)
            {
                shellGrowth = ModAssets.Amounts.ShellGrowth.Lookup(gameObject);
                kbac = master.GetComponent<KBatchedAnimController>();
                //growingGrowthModifier = new AttributeModifier(shellGrowth.amount.deltaAttribute.Id, def.defaultGrowthRate * 100f, STRINGS.CREATURES.MODIFIERS.ELEMENT_GROWTH_RATE.NAME);
                //stuntedGrowthModifier = new AttributeModifier(shellGrowth.amount.deltaAttribute.Id, def.defaultGrowthRate * 20f, STRINGS.CREATURES.MODIFIERS.ELEMENT_GROWTH_RATE.NAME);
            }
        }
    }
}
