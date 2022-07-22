using Backwalls.Buildings;
using FUtility;
using FUtility.FUI;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Backwalls
{
    internal class BackwallSideScreen : SideScreenContent
    {
        private GameObject selectedItemPanel;
        private GameObject togglePrefab;
        private Transform toggles;
        private ToggleGroup toggleGroup;
        private Backwall target;

        private Dictionary<Toggle, BackwallVariant> tags = new Dictionary<Toggle, BackwallVariant>();

        public override bool IsValidForTarget(GameObject target)
        {
            return target.GetComponent<Backwall>() != null;
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            titleKey = "Backwalls.STRINGS.UI.UISIDESCREENS.BACKWALLSIDESCREEN.TITLE";

            toggles = transform.Find("Contents/FacadeSelector/Scroll View/Viewport/Content");
            togglePrefab = toggles.Find("TogglePrefab").gameObject;

            toggleGroup = toggles.gameObject.AddComponent<ToggleGroup>();
            //toggleGroup.allowSwitchOff = true;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            gameObject.SetActive(true);
            RefreshUI();
        }

        public override void SetTarget(GameObject target)
        {
            this.target = target.GetComponent<Backwall>();

            var variant = this.target.variant;

            foreach(var tag in tags)
            {
                if(tag.Value == variant)
                {
                    tag.Key.isOn = true;
                }
            }
        }

        private void RefreshUI()
        {
            Mod.variants = Mod.variants
                .OrderBy(v => v.sortOrder)
                .ThenBy(v => v.name)
                .ToList();

            foreach (var variant in Mod.variants)
            {
                var newToggle = Instantiate(togglePrefab, toggles);
                newToggle.transform.Find("Image").GetComponent<Image>().sprite = variant.UISprite;
                newToggle.SetActive(true);

                var toggle = newToggle.GetComponent<Toggle>();
                toggle.onValueChanged.AddListener(OnToggleChosen);

                tags.Add(toggle, variant);

                toggleGroup.RegisterToggle(toggle);
                toggle.group = toggleGroup;

                Helper.AddSimpleToolTip(newToggle, variant.name);
            }
        }

        private void OnToggleChosen(bool on)
        {
            if(on)
            {
                var activeToggle = toggleGroup.GetFirstActiveToggle();

                if(activeToggle == null)
                {
                    return;
                }

                if (tags.TryGetValue(activeToggle, out var variant))
                {
                    target.SetVariant(variant);
                }
            }
        }
    }
}
