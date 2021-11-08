using FUtility.FUI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DecorPackA.Buildings.Aquarium
{
    class AquariumSideScreen : SideScreenContent
    {
        FishReceptable target;

        private FButton fishChoiceButtonPrefab;
        private Transform fishGrid;
        private GameObject decorationChoiceButtonPrefab;
        private Transform grid;

        public override bool IsValidForTarget(GameObject target) => target.GetComponent<FishReceptable>();

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            titleKey = "SpookyPumpkinSO.STRINGS.UI.UISIDESCREENS.GHOSTSIDESCREEN.TITLE";
            Helper.ListChildren(transform);
            Helper.ListComponents(gameObject);

            FindObjects();
        }

        private void FindObjects()
        {
            fishGrid = transform.Find("Contents/SelectFish/Viewport/Content");
            fishChoiceButtonPrefab = fishGrid.Find("Panel").gameObject.AddComponent<FButton>();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            LocText fishLabel = fishChoiceButtonPrefab.GetComponent<LocText>();
            if(fishLabel is object)
            {
                fishLabel.SetText("TEST");
            }

            if(fishChoiceButtonPrefab.TryGetComponent(out Text text))
            {
                text.text = "FAIL";
            }
        }

        public override void SetTarget(GameObject target)
        {
            base.SetTarget(target);
            this.target = target.GetComponent<FishReceptable>();
            gameObject.SetActive(true);
        }

        private void GenerateStateButtons()
        {
        }
    }
}
