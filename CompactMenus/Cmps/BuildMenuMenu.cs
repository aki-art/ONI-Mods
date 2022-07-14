using FUtility;
using FUtility.FUI;
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace CompactMenus.Cmps
{
    public class BuildMenuMenu : KScreen
    {
        private Toggle showNamesToggle;
        private FSlider sizeSlider;
        private GridLayoutGroup grid;

        // field accessors
        private static AccessTools.FieldRef<PlanScreen, bool> ref_categoryPanelSizeNeedsRefresh;
        private static AccessTools.FieldRef<Vector2> ref_standardBuildingButtonSize; 

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            SetupFieldAccessors();
            SetReferences();
        }

        private static void SetupFieldAccessors()
        {
            var f_standardBuildingButtonSize = typeof(PlanScreen).GetField("standarduildingButtonSize", BindingFlags.NonPublic | BindingFlags.Static);
            ref_standardBuildingButtonSize = AccessTools.StaticFieldRefAccess<Vector2>(f_standardBuildingButtonSize);

            ref_categoryPanelSizeNeedsRefresh = AccessTools.FieldRefAccess<PlanScreen, bool>("categoryPanelSizeNeedsRefresh");
        }

        private void SetReferences()
        {
            showNamesToggle = transform.Find("ShowNames").gameObject.AddComponent<FToggle>().toggle;
            sizeSlider = transform.Find("Slider").gameObject.AddComponent<FSlider>();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            showNamesToggle.onValueChanged.AddListener(OnShowNameChanged);
            sizeSlider.OnChange += OnSizeChanged;

            if(grid == null)
            {
                var f_subgroupPrefab = typeof(PlanScreen).GetField("subgroupPrefab", BindingFlags.NonPublic | BindingFlags.Instance);

                if(f_subgroupPrefab == null)
                {
                    Log.Warning("subgroupPrefab is null.");
                    return;
                }

                var subgroupPrefab = f_subgroupPrefab.GetValue(PlanScreen.Instance) as GameObject;
                grid = subgroupPrefab.GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("Grid");
            }
        }

        private void OnShowNameChanged(bool value)
        {
            Mod.Settings.BuildMenu.ShowTitles = value;
            RefreshCardSize();
            UpdatePlanScreen();
        }

        private static void UpdatePlanScreen()
        {
            ref_categoryPanelSizeNeedsRefresh(PlanScreen.Instance) = true;
            PlanScreen.Instance.ScreenUpdate(false);
        }

        private void OnSizeChanged()
        {
            Mod.Settings.BuildMenu.Scale = sizeSlider.Value;
            RefreshCardSize();
            RecalculateColumnCount();
            UpdatePlanScreen();
        }

        private void RecalculateColumnCount()
        {
            grid.constraintCount = Mathf.FloorToInt(320f / (Mod.Settings.BuildMenu.CardWidth + grid.spacing.y)) - 1;
        }

        private static void RefreshCardSize()
        {
            ref_standardBuildingButtonSize() = new Vector2(Mod.Settings.BuildMenu.CardWidth, Mod.Settings.BuildMenu.CardHeight);
        }
    }
}
