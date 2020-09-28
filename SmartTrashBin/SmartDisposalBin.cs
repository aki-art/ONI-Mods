using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SmartTrashBin
{
    [SerializationConfig(MemberSerialization.OptIn)]
    class SmartDisposalBin : StateMachineComponent<SmartDisposalBin.StatesInstance>, ISingleSliderControl, ISliderControl
    {
        [SerializeField]
        public Color noFilterTint = FilteredStorage.NO_FILTER_TINT;

        [SerializeField]
        public Color filterTint = FilteredStorage.FILTER_TINT;

        Dictionary<Tag, FetchChore> fetchChores;
        private Storage storage;
        private TreeFilterable filterable;
        public float minMass = 2000;

        #region slider
        public string SliderTitleKey => "title";
        public string SliderUnits => UI.UNITSUFFIXES.MASS.KILOGRAM;
        public int SliderDecimalPlaces(int index) => 0;

        public float GetSliderMin(int index) => 0;

        public float GetSliderMax(int index) => 1000000;

        public float GetSliderValue(int index) => minMass;

        public void SetSliderValue(float percent, int index) => minMass = percent;

        public string GetSliderTooltipKey(int index) => "tooltipkey";

        public string GetSliderTooltip() => "tooltipkey";
        #endregion

        protected override void OnSpawn()
        {
            base.OnSpawn();

            fetchChores = new Dictionary<Tag, FetchChore>();
            storage = gameObject.GetComponent<Storage>();
            filterable = gameObject.GetComponent<TreeFilterable>();
            filterable.OnFilterChanged += OnFilterChanged;

            smi.StartSM();
        }


        private void OnFilterChanged(Tag[] tags)
        {
            bool hasTags = tags != null && tags.Length != 0;
            gameObject.GetComponent<KBatchedAnimController>().TintColour = (hasTags ? filterTint : noFilterTint);
            smi.sm.filtersSet.Set(hasTags, smi);
            smi.RefreshChore();
        }

        public class StatesInstance : GameStateMachine<States, StatesInstance, SmartDisposalBin, object>.GameInstance
        {
            private FetchChore chore;
            public StatesInstance(SmartDisposalBin smi) : base(smi)
            {
            }

            public void RefreshChore() => GoTo(sm.unOperational);

            internal void EmptyAll()
            {
                foreach (GameObject item in master.storage.items)
                {
                    Util.KDestroyGameObject(item);
                }
            }

            public void CreateChore()
            {
                Debug.Log("creating chore");
                Tag[] tags = master.filterable.GetTags();

                if (tags == null || tags.Length == 0)
                {
                    GetComponent<KBatchedAnimController>().TintColour = master.noFilterTint;
                    return;
                }
                else
                {
                    GetComponent<KBatchedAnimController>().TintColour = master.filterTint;
                }

                if(master.fetchChores == null)
                {
                    master.fetchChores = new Dictionary<Tag, FetchChore>();
                }
                master.fetchChores.Clear();
                foreach (var tag in tags)
                {
                    Debug.Log(tag);
                    float surplusAmount = WorldInventory.Instance.GetAmount(tag) - 2000;
                    Debug.Log("there is an extra " + surplusAmount);
                    master.fetchChores[tag] = new FetchChore(Db.Get().ChoreTypes.StorageFetch, master.storage, float.PositiveInfinity, new Tag[] { tag });
                    //chore.AddPrecondition(IsThereSurplus, tag);
                }
            }

            public void CancelChore()
            {
                if (chore != null)
                {
                    chore.Cancel("Storage Changed");
                    chore = null;
                }
            }

            internal void RefreshChores()
            {
                //throw new NotImplementedException();
            }

            public static readonly Chore.Precondition IsThereSurplus = new Chore.Precondition
            {
                id = "IsThereSurplus",
                description = "",
                fn = delegate (ref Chore.Precondition.Context context, object data)
                {
                    float amount = WorldInventory.Instance.GetAmount((Tag)data);
                    Debug.Log("checked for surplus on " + (Tag)data);
                    Debug.Log(amount);

                    return amount >= 2000;
                }
            };
        }

        public class States : GameStateMachine<States, StatesInstance, SmartDisposalBin>
        {
            public State unOperational;
            public State unset;
            public State collecting;
            public State unlimited;
            public BoolParameter filtersSet;

            public override void InitializeStates(out BaseState default_state)
            {
                default_state = unOperational;

                unOperational
                    .Enter(smi => Debug.Log("unOperational"))
                    .TagTransition(GameTags.Operational, unset);
                unset
                    .Enter(smi => Debug.Log("unset"))
                    .ParamTransition(filtersSet, collecting, IsTrue)
                    .TagTransition(GameTags.Operational, unOperational, true);
                collecting
                    //.Update((smi, dt) => smi.RefreshChores(), UpdateRate.SIM_1000ms)
                    //.Update((smi, dt) => smi.EmptyAll(), UpdateRate.SIM_200ms)
                    .Enter("CreateChore", smi => smi.CreateChore())
                    .Exit("CancelChore", smi => smi.CancelChore())
                    .ParamTransition(filtersSet, unOperational, IsFalse)
                    .TagTransition(GameTags.Operational, unOperational, true);
            }

        }
    }
}
