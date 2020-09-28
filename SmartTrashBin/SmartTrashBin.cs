using Harmony;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartTrashBin
{
    class SmartTrashBin : KMonoBehaviour, ISingleSliderControl, ISliderControl, ISim1000ms
    {
        [MyCmpReq]
        Storage storage;
        [MyCmpReq]
        TreeFilterable treeFilterable;
        Traverse forbiddenTags;

        public float minMass = 200000;

        FilteredTrash filteredStorage;

        public string SliderTitleKey => "title";

        public string SliderUnits => UI.UNITSUFFIXES.MASS.KILOGRAM;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            filteredStorage = new FilteredTrash(this, null, null, Db.Get().ChoreTypes.StorageFetch);
            storage.Subscribe((int)GameHashes.OnStore, OnStore);
            forbiddenTags = Traverse.Create(filteredStorage).Field("forbiddenTags");
           // treeFilterable.OnFilterChanged += UpdateFilters;

        }

        private void OnStore(object obj)
        {
            Debug.Log("something was stored");
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            filteredStorage.FilterChanged();
        }

        public void Sim1000ms(float dt)
        {
            filteredStorage.UpdateTags(GetSliderValue(0));
        }


        public int SliderDecimalPlaces(int index) => 0;

        public float GetSliderMin(int index) => 0;

        public float GetSliderMax(int index) => 1000000;

        public float GetSliderValue(int index) => minMass;

        public void SetSliderValue(float percent, int index) => minMass = percent;

        public string GetSliderTooltipKey(int index) => "tooltipkey";

        public string GetSliderTooltip() => "tooltipkey";
    }
}
