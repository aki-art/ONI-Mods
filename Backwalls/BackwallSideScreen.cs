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

        private Dictionary<Toggle, string> tags = new Dictionary<Toggle, string>();

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

            var id = this.target.tag;

            foreach(var tag in tags)
            {
                if(tag.Value == id)
                {
                    tag.Key.isOn = true;
                }
            }
        }

        private void OnButtonClick()
        {
        }

        struct TileEntry
        {
            public BuildingDef def;
            public int sortOrder;
        }

        private void RefreshUI()
        {
            var tiles = new List<TileEntry>();

            foreach(var def in Assets.BuildingDefs.Where(def => def.BlockTileAtlas != null))
            {
                var sortOrder = 0;
                if(def.PrefabID == TileConfig.ID)
                {
                    sortOrder = -1;
                }
                else if(def.BuildingComplete.HasTag("DecorPackA_StainedGlass"))
                {
                    sortOrder = 2;
                }
                tiles.Add(new TileEntry()
                {
                    def = def,
                    sortOrder = sortOrder
                });
            }

            tiles.Sort((t1, t2) => t1.sortOrder.CompareTo(t2.sortOrder));

            foreach (var tile in tiles)
            {
                var def = tile.def;
                var newToggle = Instantiate(togglePrefab, toggles);
                newToggle.transform.Find("Image").GetComponent<Image>().sprite = def.GetUISprite();
                newToggle.SetActive(true);

                var toggle = newToggle.GetComponent<Toggle>();
                toggle.onValueChanged.AddListener(OnToggleChosen);

                tags.Add(toggle, def.PrefabID);

                toggleGroup.RegisterToggle(toggle);
                toggle.group = toggleGroup;

                Helper.AddSimpleToolTip(newToggle, def.Name);
            }
        }

        private void OnToggleChosen(bool on)
        {
            Log.Debuglog("on toggle chosen " + on);
            if(on)
            {
                var activeToggle = toggleGroup.GetFirstActiveToggle();

                if(activeToggle == null)
                {
                    Log.Debuglog("activeToggle is null");
                    return;
                }

                if (tags.TryGetValue(activeToggle, out string tag))
                {
                    target.SetDef(tag);
                }
            }
        }
    }
}
