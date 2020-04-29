/*
using Klei;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Slag.Buildings
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class FiltrationTile4 : StateMachineComponent<FiltrationTile4.StatesInstance>, IEffectDescriptor
    {
        private const float MAX_PRESSURE = 2000f;
        private const float SPILL_RATE = 20f;
        public float MAX_BUILDUP = 500f;

        [MyCmpGet]
        [NonSerialized]
        public Structure structure;
        [MyCmpGet]
        public Operational operational;
        [MyCmpGet]
        private PassiveElementConsumer elementConsumer;
        private Storage liquidStorage;
        private Storage solidStorage;

        [SerializeField]
        public List<Tag> filteredElements;

        [SerializeField]
        public Endpoint endpointType = Endpoint.Source;

        private FiltrationTileWorkable filtrationTileWorkable;

        public CellOffset choreOffset = new CellOffset(0, 1);


        public Tag buildupTag = GameTags.Solid;

        private int cell = -1;

        KAnimFile[] anim_overrides = new KAnimFile[] { Assets.GetAnim("anim_interacts_outhouse_kanim") };
        private static readonly HashedString[] CLEAN_ANIMS = new HashedString[]  { "unclog_pre", "unclog_loop" };
        private static readonly HashedString PST_ANIM = new HashedString("unclog_pst");

        public enum State
        {
            Invalid,
            Ready,
            Blocked,
            OverPressure
        }
        protected override void OnSpawn()
        {
            base.OnSpawn();

            cell = Grid.CellBelow(Grid.PosToCell(transform.GetPosition()));

            var converters = GetComponents<ElementConverter>();
            foreach (var converter in converters)
            {
                var filterables = new List<ElementConverter.ConsumedElement>(converter.consumedElements).Select(e => e.tag).ToList();
                filteredElements.AddRange(filterables);
            }

            foreach (var storage in GetComponents<Storage>())
            {
                if (storage.storageFilters.Contains(GameTags.Liquid))
                    liquidStorage = storage;
                else if (storage.storageFilters.Contains(GameTags.Solid))
                    solidStorage = storage;
                else Debug.Log("FiltrationTile: This storage has neither liquid or solid tag.");
            }

            var top = new CellOffset[] { new CellOffset(0, 1) };
            filtrationTileWorkable = GetComponent<FiltrationTileWorkable>();
            filtrationTileWorkable.SetOffsetTable(new CellOffset[][] { top });
            filtrationTileWorkable.workTime = 20f;
            filtrationTileWorkable.overrideAnims = anim_overrides;
            filtrationTileWorkable.workLayer = Grid.SceneLayer.BuildingFront;

            

            smi.StartSM();
        }

        private void EmitLiquid(int cell)
        {
            bool emitParticle = (Grid.IsValidCell(cell) && !Grid.Solid[cell]);
            foreach (GameObject stored_item in liquidStorage.items)
            {
                PrimaryElement primary_element = stored_item.GetComponent<PrimaryElement>();
                if (primary_element.Element.IsLiquid && !filteredElements.Contains(primary_element.Element.tag))
                {
                    if (EmitCommon(cell, primary_element, emitParticle))
                    {
                        break;
                    }
                }
            }
        }
        private bool EmitCommon(int cell, PrimaryElement primary_element, bool emitParticle)
        {
            bool result;
            if (primary_element.Mass <= 0f)
                result = false;
            else
            {
                float spill = Mathf.Min(primary_element.Mass, SPILL_RATE);
                liquidStorage.ConsumeAndGetDisease(primary_element.ElementID.CreateTag(), spill, out SimUtil.DiseaseInfo diseaseInfo, out float temperature);

                if (emitParticle)
                {
                    FallingWater.instance.AddParticle(
                    cell: cell,
                    elementIdx: (byte)ElementLoader.elements.IndexOf(primary_element.Element),
                    base_mass: spill,
                    temperature: temperature,
                    disease_idx: diseaseInfo.idx,
                    base_disease_count: diseaseInfo.count,
                    skip_sound: true,
                    skip_decor: false,
                    debug_track: true);
                }
                else
                {
                    SimMessages.AddRemoveSubstance(
                    gameCell: cell,
                    new_element: primary_element.ElementID,
                    ev: CellEventLogger.Instance.ExhaustSimUpdate,
                    mass: spill,
                    temperature: temperature,
                    disease_idx: diseaseInfo.idx,
                    disease_count: diseaseInfo.count);
                }

                result = true;
            }

            return result;
        }

        private void ProcessLiquid()
        {
            if (liquidStorage.items.Count != 0)
            {
                if (!Grid.Solid[cell])
                {
                    EmitLiquid(cell);
                }
            }
        }

        protected override void OnCleanUp()
        {
            UpdateChores(false);
            base.OnCleanUp();
        }
        public float GetBuildUp()
        {
            List<GameObject> items = solidStorage.items;

            float totalMass = 0f;

            foreach (var item in items)
            {
                if (item != null)
                {
                    if (item.HasTag(GameTags.Solid))
                    {
                        totalMass += item.GetComponent<PrimaryElement>().Mass;
                    }
                }
            }

            return totalMass;
        }
        public State GetEndPointState()
        {
            State new_state = State.Invalid;
            Endpoint endpoint = endpointType;
            if (endpoint != Endpoint.Source)
            {
                if (endpoint == Endpoint.Sink)
                {
                    new_state = State.Ready;
                    int output_cell = cell;
                    bool valid_output_cell = IsValidOutputCell(output_cell);
                    if (!valid_output_cell)
                    {
                        new_state = (Grid.Solid[output_cell] ? State.Blocked : State.OverPressure);
                    }
                }
            }
            else
            {
                new_state = State.Ready;
            }
            return new_state;
        }

        private bool IsValidOutputCell(int output_cell)
        {
            bool valid = false;
            if (structure == null || !structure.IsEntombed())
            {
                if (!Grid.Solid[output_cell])
                {
                    valid = (Grid.Mass[output_cell] < MAX_PRESSURE);
                }
            }
            return valid;
        }
        private Chore CreateChore(int i)
        {
            return null;
        }

        private void OnCleanChoreEnd(Chore chore)
        {
            if (gameObject.HasTag(GameTags.Operational))
            {
                UpdateChores(true);
            }
        }

        public void UpdateChores(bool update = true)
        {
        }

        List<Descriptor> IEffectDescriptor.GetDescriptors(BuildingDef def)
        {
            List<Descriptor> descs = new List<Descriptor>();
            Descriptor desc = default;
            desc.SetupDescriptor(UI.BUILDINGEFFECTS.RECREATION, UI.BUILDINGEFFECTS.TOOLTIPS.RECREATION, Descriptor.DescriptorType.Effect);
            descs.Add(desc);
            return descs;
        }
        public bool HasEnoughMass(Tag tag)
        {
            // calculate if we can start filtering
            return true;
        }
        public class States : GameStateMachine<States, StatesInstance, FiltrationTile4>
        {

            public State waiting; 
            public State filtering;
            public State needsEmptying;
            public State notoperational;
            public State blocked;
            public State overPressure;
            public override void InitializeStates(out BaseState default_state)
            {
                default_state = waiting;
                root
				    .EventTransition(GameHashes.OperationalChanged, notoperational, smi => !smi.master.operational.IsOperational)
                    .EventTransition(GameHashes.OperationalChanged, waiting, smi => smi.master.operational.IsOperational);
                notoperational
                    .QueueAnim("off", false, null);
                waiting
                    .Enter(smi => smi.master.elementConsumer.EnableConsumption(true))
                    .QueueAnim("on", false)
                    .EventTransition(GameHashes.OnStorageChange, filtering, smi => smi.HasEnoughMass(GameTags.Liquid))
                    .EventTransition(GameHashes.OnStorageChange, needsEmptying, smi => smi.HasEnoughMass(GameTags.Solid));
                needsEmptying
                    .Enter(smi => smi.master.elementConsumer.EnableConsumption(false))
                    .QueueAnim("off", false)
                    .EventTransition(GameHashes.OnStorageChange, waiting, smi => !smi.HasEnoughMass(GameTags.Solid));
                filtering
                    .Enter(smi => smi.master.operational.SetActive(true))
                    .Update("Filtering", (smi, dt) => { smi.Filter(dt); }, UpdateRate.SIM_200ms) // goes to blocked or overpressured from update with GOTO
                    .Exit(smi => smi.master.operational.SetActive(false))
                    .Exit(smi => smi.master.elementConsumer.EnableConsumption(false))
                    .QueueAnim("working_loop", true);
                blocked
                    .ToggleStatusItem(smi => Db.Get().BuildingStatusItems.LiquidVentObstructed, null)
                    .Update((smi, dt) => { smi.CheckTransitions(); }, UpdateRate.SIM_1000ms);
                overPressure
                    .ToggleStatusItem(smi => Db.Get().BuildingStatusItems.LiquidVentOverPressure, null)
                    .Update((smi, dt) => { smi.CheckTransitions(); }, UpdateRate.SIM_1000ms);
            }

        }

        public class StatesInstance : GameStateMachine<States, StatesInstance, FiltrationTile4, object>.GameInstance
        {
            Operational operational;
            public StatesInstance(FiltrationTile4 smi) : base(smi)
            {
                operational = master.GetComponent<Operational>();
            }
            public bool Blocked()
            {
                return master.GetEndPointState() == State.Blocked && master.endpointType > Endpoint.Source;
            }
            public bool OverPressure()
            {
                return master.GetEndPointState() == State.OverPressure && master.endpointType > Endpoint.Source;
            }
            public bool Clogged()
            {
                return master.GetBuildUp() >= master.MAX_BUILDUP;
            }
            public void SetActive(bool active)
            {
                operational.SetActive(this.operational.IsOperational && active, false);
            }

            internal void CheckTransitions()
            {
                if (OverPressure() || Blocked() || Clogged())
                {
                    CheckStates();
                }
                else smi.GoTo(sm.waiting);
            }

            internal void Filter(float dt)
            {
                CheckStates();
                master.ProcessLiquid();
            }

            private void CheckStates()
            {
                if (OverPressure())
                {
                    smi.GoTo(sm.overPressure);
                    return;
                }
                else if (Blocked())
                {
                    smi.GoTo(sm.blocked);
                    return;
                }
                else if (Clogged())
                {
                    smi.GoTo(sm.needsEmptying);
                    return;
                }
            }

            internal bool HasEnoughMass(Tag solid)
            {
                throw new NotImplementedException();
            }
        }
    }

}
*/