using FUtility.FUI;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace CompactMenus.Patches
{
    public class PlanScreenPatches
    {
        [HarmonyPatch(typeof(PlanBuildingToggle), "RefreshBuildingButtonIconAndColors")]
        public class PlanBuildingToggle_RefreshBuildingButtonIconAndColors_Patch
        {
            public static void Postfix(Image ___buildingIcon)
            {
                // make UI image smol
                var rectTransform = ___buildingIcon.rectTransform();

                rectTransform.sizeDelta /= 1.75f;
                rectTransform.position = Vector3.zero;

                rectTransform.pivot = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

                if (Mod.Settings.BuildMenu.ShowTitles)
                {
                    //rectTransform.localPosition = new Vector3(0, rectTransform.sizeDelta.y, -0.1f);
                    rectTransform.localPosition = new Vector2(0, 0f);
                }
                else
                {
                    rectTransform.localPosition = new Vector2(0, 0f);
                }
            }
        }

        [HarmonyPatch(typeof(PlanBuildingToggle), "Config")]
        public class PlanBuildingToggle_Config_Patch
        {
            public static void Postfix(LocText ___text)
            {
                // configure titles showing
                ___text.enabled = Mod.Settings.BuildMenu.ShowTitles;

                if(Mod.Settings.BuildMenu.ShowTitles)
                {
                    //___text.fontSize *= Mod.Settings.BuildMenu.Scale;
                    ___text.rectTransform().sizeDelta = new Vector2(___text.rectTransform().sizeDelta.x, Mathf.Min(___text.rectTransform().sizeDelta.y, 5f));
                }
            }
        }

        [HarmonyPatch(typeof(PlanBuildingToggle), "RefreshLabel")]
        public class PlanBuildingToggle_RefreshLabel_Patch
        {
            public static void Postfix(LocText ___text)
            {
                // configure titles showing
                ___text.enabled = Mod.Settings.BuildMenu.ShowTitles;

                if (Mod.Settings.BuildMenu.ShowTitles)
                {
                    //___text.fontSize *= Mod.Settings.BuildMenu.Scale;
                    ___text.rectTransform().sizeDelta = new Vector2(___text.rectTransform().sizeDelta.x, Mathf.Min(___text.rectTransform().sizeDelta.y, 5f));
                }
            }
        }


        [HarmonyPatch(typeof(PlanScreen), "ConfigurePanelSize")]
        public class PlanScreen_ConfigurePanelSize_Patch
        {
            public static void Postfix(PlanScreen __instance)
            {
                //__instance.buildingGroupsRoot.sizeDelta += new Vector2(0, 34f);
            }
        }

        [HarmonyPatch(typeof(PlanScreen), "OnPrefabInit")]
        public class PlanScreen_OnPrefabInit_Patch
        {
            public static void Prefix(PlanScreen __instance, ref bool ___USE_SUB_CATEGORY_LAYOUT, GameObject ___subgroupPrefab)
            {
                // enable grouping
                if (Mod.Settings.BuildMenu.UseSubCategories)
                {
                    ___USE_SUB_CATEGORY_LAYOUT = true;
                }

                if(ModAssets.inputFieldPrefab == null)
                {
                    ModAssets.CreatePrefabs();
                }

                var parent = __instance.GroupsTransform.parent.parent;

                var container = new GameObject("searchbox_container");
                container.transform.SetParent(parent);

                var transform = container.AddComponent<RectTransform>();
                transform.localScale = Vector3.one;
                transform.anchorMin = new Vector2(0, 1);
                transform.anchorMax = new Vector2(0, 1);
                transform.pivot = new Vector2(0, 1);
                transform.localPosition = Vector3.zero;
                transform.anchoredPosition = new Vector3(0f, 0f, parent.position.y - 0.5f); 
                transform.sizeDelta = new Vector2(260f, 30f);

                container.SetLayerRecursively(5);
                container.AddComponent<LayoutElement>().minHeight = 30f;
                container.transform.SetSiblingIndex(3); //after title, before grid

                container.SetActive(true);

                //var go = Object.Instantiate(ModAssets.inputFieldPrefab);
                var go = Util.KInstantiateUI(ModAssets.inputFieldPrefab, container, true);

                // add settings button
                var titleBar = parent.Find("TitleBar");

                var menuButton = Object.Instantiate(ModAssets.menuIconPrefab);
                menuButton.transform.SetParent(titleBar);
                menuButton.transform.localPosition = Vector3.zero;
                menuButton.SetActive(true);

                var settingPanel = Object.Instantiate(ModAssets.buildMenuMenuPrefab);
                settingPanel.transform.SetParent(__instance.BuildingGroupContentsRect);
                settingPanel.transform.SetSiblingIndex(3);
                settingPanel.transform.localPosition = new Vector3(0, 0);
                settingPanel.SetActive(false);

                var menuToggle = menuButton.AddComponent<FToggle>();
                menuToggle.OnClick += () => settingPanel.SetActive(menuToggle.toggle.isOn);

                // make cards tiny
                typeof(PlanScreen)
                    .GetField("standarduildingButtonSize", BindingFlags.NonPublic | BindingFlags.Static)
                    .SetValue(null, new Vector2(Mod.Settings.BuildMenu.CardWidth, Mod.Settings.BuildMenu.CardHeight));

                // adjust column count
                var grid = ___subgroupPrefab.GetComponent<HierarchyReferences>().GetReference<GridLayoutGroup>("Grid");
                grid.constraintCount = Mathf.FloorToInt(320f / (Mod.Settings.BuildMenu.CardWidth + grid.spacing.y)) - 1;

                __instance.buildingGroupsRoot.localPosition += new Vector3(0, 34f);
            }
        }
    }
}
