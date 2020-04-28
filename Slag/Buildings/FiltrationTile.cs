using Klei;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Slag.Buildings
{
    class FiltrationTile : KMonoBehaviour
    {
        [MyCmpGet]
        [NonSerialized]
        public Structure structure;
        [MyCmpGet]
        private Storage storage;
        [MyCmpGet]
        private PassiveElementConsumer elementConsumer;
        [MyCmpGet]
        private Operational operational;
        [MyCmpGet]
        private FiltrationTileWorkable filtrationTileWorkable;

        private StatesInstance smi;
        CellOffset choreOffset;
        private static FiltrationTileWorkable workable;
        protected OffsetTracker offsetTracker;

        private const float MAX_PRESSURE = 2000f;
        private const float SPILL_RATE = 20f;
        public float MAX_BUILDUP = 500f;

        private int cell = -1;

        public Tag buildupTag = GameTags.Solid;
        public bool cloggingDisablesIntake = true;

        [SerializeField]
        public List<Tag> filteredElements;

        [SerializeField]
        public Endpoint endpointType = Endpoint.Source;


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

            filtrationTileWorkable = GetComponent<FiltrationTileWorkable>();
            var top = new CellOffset[][]
            {
                new CellOffset[]  { new CellOffset(0, 1) }
            };
            filtrationTileWorkable.SetOffsetTable(top);

            KAnimFile[] anim_overrides = new KAnimFile[]
            {
                Assets.GetAnim("anim_interacts_outhouse_kanim")
            };

            filtrationTileWorkable.workTime = 20f;
            filtrationTileWorkable.overrideAnims = anim_overrides;
            filtrationTileWorkable.workLayer = Grid.SceneLayer.BuildingFront;

            smi = new StatesInstance(this);
            smi.StartSM();
        }


        public float GetBuildUp()
        {
            List<GameObject> items = storage.items;
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
        private void EmitLiquid(int cell)
        {
            bool emitParticle = (Grid.IsValidCell(cell) && !Grid.Solid[cell]);
            foreach (GameObject stored_item in storage.items)
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
                storage.ConsumeAndGetDisease(primary_element.ElementID.CreateTag(), spill, out SimUtil.DiseaseInfo diseaseInfo, out float temperature);

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

        private void UpdateEmission()
        {
            if (storage.items.Count != 0)
            {
                if (!Grid.Solid[cell])
                {
                    EmitLiquid(cell);
                }
            }
        }


        public class StatesInstance : GameStateMachine<States, StatesInstance, FiltrationTile, object>.GameInstance
        {
            public Chore cleanChore;
            public StatesInstance(FiltrationTile master) : base(master)
            {
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
            public void UpdateEmissions()
            {
                master.UpdateEmission();
            }
            public void CheckTransitions()
            {
                if (Clogged())
                {
                    smi.GoTo(sm.clogged);
                }
                else
                {
                    if (Blocked())
                    {
                        smi.GoTo(sm.blocked);
                    }
                    else
                    {
                        if (OverPressure())
                        {
                            smi.GoTo(sm.overPressure);
                        }
                        else
                        {
                            smi.GoTo(sm.idle);
                        }
                    }
                }
                UpdateEmissions();
            }

            public void ClogUp()
            {
                Debug.Log("ClogUp");
                master.elementConsumer.EnableConsumption(false);
                //master.operational.SetActive(false, false);
                CreateCleanChore();
                //cleanChore = CreateChore();
            }
 
            public void CreateCleanChore()
            {
                if (cleanChore != null)
                {
                    cleanChore.Cancel("dupe");
                }

                if (master.filtrationTileWorkable == null) Debug.Log("filtrationTileWorkable is null");

                cleanChore = new WorkChore<FiltrationTileWorkable>(
                    chore_type: Db.Get().ChoreTypes.EmptyStorage,
                    target: master.filtrationTileWorkable,
                    chore_provider: null,
                    run_until_complete: true,
                    on_complete: null, //new Action<Chore>(OnCleanComplete),
                    on_begin: null,
                    on_end: new Action<Chore>(OnCleanComplete),
                    allow_in_red_alert: true,
                    schedule_block: null,
                    ignore_schedule_block: false,
                    only_when_operational: true,
                    override_anims: master.filtrationTileWorkable.overrideAnims[0],
                    is_preemptable: false,
                    allow_in_context_menu: true,
                    allow_prioritization: true,
                    priority_class: PriorityScreen.PriorityClass.basic,
                    priority_class_value: 5,
                    ignore_building_assignment: true,
                    add_to_daily_report: true);
            }
            public void CancelCleanChore()
            {
                if (cleanChore != null)
                {
                    cleanChore.Cancel("Cancelled");
                    cleanChore = null;
                }
            }
            private void OnCleanComplete(Chore chore)
            {
                Debug.Log("OnCleanComplete");
                if (master.storage == null) return;

                List<GameObject> items = master.storage.items;

                foreach (var item in items)
                {
                    if (item != null)
                    {
                        if (item.HasTag(GameTags.Solid))
                        {
                            master.storage.Drop(item, true);
                        }
                    }
                }

                //base.master.meter.SetPositionPercent((float)base.master.FlushesUsed / (float)base.master.maxFlushes);
            }
        }

        public class States : GameStateMachine<States, StatesInstance, FiltrationTile>
        {
            public State idle;
            public State blocked;
            public State overPressure;
            public State clogged;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = idle;
                root
                    .Update("CheckTransitions", delegate (StatesInstance smi, float dt) { smi.CheckTransitions(); }, UpdateRate.SIM_200ms, false);
                idle
                    .Enter(smi => smi.master.elementConsumer.EnableConsumption(true))
                    .Exit(smi => smi.master.elementConsumer.EnableConsumption(false))
                    .Enter(smi => smi.master.operational.SetActive(true, false));
                // .Exit(smi => smi.master.operational.SetActive(false, false));
                blocked
                    .ToggleStatusItem(Db.Get().BuildingStatusItems.LiquidVentObstructed);
                overPressure
                    .ToggleStatusItem(Db.Get().BuildingStatusItems.LiquidVentOverPressure);
                clogged
                    .ToggleStatusItem(Db.Get().BuildingStatusItems.ToiletNeedsEmptying)
                    .Enter(smi => smi.ClogUp())
                    //.ToggleChore(smi => smi.cleanChore, idle);
                    .Exit(smi => smi.CancelCleanChore());
            }
        }
    }
}
