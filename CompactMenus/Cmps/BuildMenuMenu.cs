using FUtility;
using FUtility.FUI;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace CompactMenus.Cmps
{
    public class BuildMenuMenu : KScreen
    {
        public static float BgWidth => ScreenResolutionMonitor.UsingGamepadUIMode() ? 320f : 264f;

        public const float BORDER = 28f;

        public static float WidthBorderless => BgWidth - BORDER;

        private Toggle showNamesToggle;
        private Slider sizeSlider;
        private GridLayoutGroup grid;

        public const float MIN_SCALE = 0.2786211f; // 8 cards
        public const float MAX_SCALE = 2.29f; // 1 card

        public const int MIN_COLUMNS = 2;
        public const int MAX_COLUMNS = 8;
        public const float SPACING = 4f;

        private bool preciseControl = false;

        private static Vector2 spacing = new Vector2(5f, 5f);

        // field accessors
        private static AccessTools.FieldRef<PlanScreen, bool> ref_categoryPanelSizeNeedsRefresh;
        private static AccessTools.FieldRef<Vector2> ref_standardBuildingButtonSize; 

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            SetupFieldAccessors();
            SetReferences();
        }

        public override void OnKeyDown(KButtonEvent e)
        {
            preciseControl = e.Controller.GetKeyDown(KKeyCode.LeftShift) || e.Controller.GetKeyDown(KKeyCode.RightShift);

            base.OnKeyDown(e);
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
            sizeSlider = transform.Find("Slider/Slider").gameObject.AddComponent<FSlider>().slider;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            showNamesToggle.onValueChanged.AddListener(OnShowNameChanged);
            sizeSlider.minValue = 2;
            sizeSlider.maxValue = 8;
            sizeSlider.direction = Slider.Direction.RightToLeft;
            sizeSlider.wholeNumbers = true;
            sizeSlider.value = Mod.Settings.BuildMenu.Scale; //Remap(Mod.Settings.BuildMenu.Scale, minScale, maxScale, 0, 1);
            sizeSlider.onValueChanged.AddListener(OnSizeChanged);

            showNamesToggle.isOn = Mod.Settings.BuildMenu.ShowTitles;

            if (grid == null)
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

            Helper.AddSimpleToolTip(sizeSlider.transform.parent.gameObject, STRINGS.UI.BUILDMENUMENU.CARDSIZE.TOOLTIP);
            Helper.AddSimpleToolTip(showNamesToggle.gameObject, STRINGS.UI.BUILDMENUMENU.SHOWNAMES.TOOLTIP);
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

            var m_ConfigurePanelSize = typeof(PlanScreen).GetMethod("ConfigurePanelSize", BindingFlags.NonPublic | BindingFlags.Instance);
            m_ConfigurePanelSize.Invoke(PlanScreen.Instance, new object[] { null });
        }

        public void OnSizeChanged(float value)
        {
            Mod.Settings.BuildMenu.Scale = MAX_SCALE / value; // Remap(value, 0, 1, minScale, maxScale);
            Mod.Settings.BuildMenu.Columns = (int)value;

            RefreshCardSize();
            RecalculateColumnCount();
            UpdatePlanScreen();
        }

        public static int GetConstraintCount(float width)
        {
            //return Mathf.FloorToInt(width / Mod.Settings.BuildMenu.CardWidth);
            return Mod.Settings.BuildMenu.Columns;
        }

        public static Vector2 GetSpacing(int constraintCount, float width)
        {
            var totalCardsWidth = constraintCount * Mod.Settings.BuildMenu.CardWidth;
            var remainingSpace = width - totalCardsWidth;
            var spacingCount = constraintCount - 1;
            var spacingDist = spacingCount == 0 ? 0f : remainingSpace / spacingCount;
            spacingDist = Mathf.Clamp(spacingDist, 0f, 100f);

            return new Vector2(spacingDist, spacingDist);

            //return spacing;
        }

        // using math instead of flexible constraint, because that would domino into a lot of other UI breaking
        public void RecalculateColumnCount()
        {
            if(PlanScreen.Instance?.GroupsTransform == null || grid == null)
            {
                return;
            }

            var width = WidthBorderless;
            //var constraintCount = GetConstraintCount(width);
            var constraintCount = Mod.Settings.BuildMenu.Columns;
            //var spacing = GetSpacing(constraintCount, width);

            grid.constraintCount = constraintCount;
            grid.spacing = spacing;

            foreach (Transform child in PlanScreen.Instance.GroupsTransform)
            {
                if(child.TryGetComponent(out HierarchyReferences hr))
                {
                    var subGrid = hr.GetReference<GridLayoutGroup>("Grid");
                    if(subGrid != null)
                    {
                        subGrid.constraintCount = constraintCount;
                        //subGrid.spacing = spacing;
                    }
                }
            }
        }

        private static void RefreshCardSize()
        {
            ref_standardBuildingButtonSize() = new Vector2(Mod.Settings.BuildMenu.CardWidth, Mod.Settings.BuildMenu.CardHeight);
        }
    }
}
