using HarmonyLib;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DecorPackA.Buildings.StainedGlassTile
{
    public class StainedGlassCodexLabel : CodexLabelWithIcon
    {
        public string element { get; set; }

        public string linkID { get; set; }

        public StainedGlassCodexLabel(string text, CodexTextStyle style, Tuple<Sprite, Color> coloredSprite, string targetEntrylinkID, string element) : base(text, style, coloredSprite, 128, 128)
        {
            icon = new CodexImage(128, 128, coloredSprite);
            label = new CodexText(text, style);
            linkID = targetEntrylinkID;
            this.element = element;
        }

        public StainedGlassCodexLabel()
        {
        }

        public override void Configure(GameObject contentGameObject, Transform displayPane, Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
        {
            var image = contentGameObject.GetComponentsInChildren<Image>()[1];

            icon.ConfigureImage(image);

            if (icon.preferredWidth != -1 && icon.preferredHeight != -1)
            {
                var layoutElement = image.GetComponent<LayoutElement>();
                layoutElement.minWidth = icon.preferredHeight;
                layoutElement.minHeight = icon.preferredWidth;
                layoutElement.preferredHeight = icon.preferredHeight;
                layoutElement.preferredWidth = icon.preferredWidth;
            }

            label.text = UI.StripLinkFormatting(label.text);
            label.ConfigureLabel(contentGameObject.GetComponentInChildren<LocText>(), textStyles);

            var kButton = contentGameObject.GetComponent<KButton>();
            kButton.ClearOnClick();
            kButton.onClick += OpenBuildMenu;
        }

        private void OpenBuildMenu()
        {
            var defaultStainedDef = Assets.GetBuildingDef(DefaultStainedGlassTileConfig.DEFAULT_ID);

            var techItem = Db.Get().TechItems.TryGet(defaultStainedDef.PrefabID);

            var canBuild = DebugHandler.InstantBuildMode ||
                (Game.Instance.SandboxModeActive && SandboxToolParameterMenu.instance.settings.InstantBuild) ||
                (techItem == null || techItem.IsComplete());

            if (!canBuild)
            {
                KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative"));
                return;
            }

            foreach (var planInfo in TUNING.BUILDINGS.PLANORDER)
            {
                foreach (var categoryData in planInfo.buildingAndSubcategoryData)
                {
                    if (categoryData.Key == DefaultStainedGlassTileConfig.DEFAULT_ID)
                    {
                        OpenMenu(defaultStainedDef, planInfo);
                        return;
                    }
                }
            }
        }

#if FAST_FRIENDS
        private void OpenMenu(BuildingDef defaultStainedDef, PlanScreen.PlanInfo planInfo)
        {
            PlanScreen.Instance.OpenCategoryByName(HashCache.Get().Get(planInfo.category));

            var gameObject = PlanScreen.Instance.ActiveCategoryBuildingToggles[defaultStainedDef].gameObject;

            PlanScreen.Instance.OnSelectBuilding(gameObject, defaultStainedDef);

            var infoScreen = PlanScreen.Instance.ProductInfoScreen;

            if (infoScreen == null)
            {
                return;
            }

            var elementTag = element;

            var infoScreenTraverse = Traverse.Create(infoScreen.materialSelectionPanel);
            var MaterialSelectors = infoScreenTraverse.Field<List<MaterialSelector>>("MaterialSelectors").Value;

            var index = 1;

            if (MaterialSelectors[index].ElementToggles.ContainsKey(elementTag))
            {
                MaterialSelectors[index].OnSelectMaterial(elementTag, null);//activeRecipe);
            }

            return;
        }
#else
        private void OpenMenu(BuildingDef defaultStainedDef, PlanScreen.PlanInfo planInfo)
        {
            PlanScreen.Instance.OpenCategoryByName(HashCache.Get().Get(planInfo.category));

            var gameObject = PlanScreen.Instance.ActiveToggles[defaultStainedDef].gameObject;

            PlanScreen.Instance.OnSelectBuilding(gameObject, defaultStainedDef);

            var infoScreen = Patches.PlanScreenPatch.PlanScreen_OnClickCopyBuilding_Patch.GetProductInfoScreen();

            if (infoScreen == null)
            {
                return;
            }

            var elementTag = element;

            var infoScreenTraverse = Traverse.Create(infoScreen.materialSelectionPanel);
            var MaterialSelectors = infoScreenTraverse.Field<List<MaterialSelector>>("MaterialSelectors").Value;

            var index = 1;

            if (MaterialSelectors[index].ElementToggles.ContainsKey(elementTag))
            {
                MaterialSelectors[index].OnSelectMaterial(elementTag, null);//activeRecipe);
            }

            return;
        }
#endif
    }
}
