using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace DecorPackA.Patches
{
    public class MaterialSelectorPatch
    {
        // colors liquid & gas icons
        [HarmonyPatch(typeof(MaterialSelector), "SetToggleBGImage")]
        public class MaterialSelector_SetToggleBGImage_Patch
        {
            public static void Postfix(KToggle toggle, Tag elem, Recipe.Ingredient ___activeIngredient)
            {
                if (___activeIngredient.tag != ModAssets.Tags.stainedGlassDye)
                {
                    return;
                }

                if (ElementLoader.GetElement(elem) is Element element && !element.IsSolid)
                {
                    toggle.gameObject.GetComponentsInChildren<Image>()[1].color = element.substance.uiColour;
                }
            }
        }

        // forces liquid and gas options without enabling them for other buildings
        [HarmonyPatch(typeof(MaterialSelector), "ConfigureScreen")]
        public static class MaterialSelector_ConfigureScreen_Patch
        {
            public static void Postfix(MaterialSelector __instance, Recipe.Ingredient ingredient, ToggleGroup ___toggleGroup)
            {
                if (ingredient.tag != ModAssets.Tags.stainedGlassDye)
                {
                    return;
                }

                foreach (var tag in ModAssets.Tags.extraGlassDyes)
                {
                    AddToggle(__instance, ___toggleGroup, tag);
                }

                __instance.RefreshToggleContents();
            }

            private static void AddToggle(MaterialSelector __instance, ToggleGroup ___toggleGroup, Tag tag)
            {
                if (!__instance.ElementToggles.ContainsKey(tag))
                {
                    var toggle = Util.KInstantiate(__instance.TogglePrefab, __instance.LayoutContainer, "MaterialSelection_" + tag.ProperName());
                    toggle.transform.localScale = Vector3.one;
                    toggle.SetActive(true);

                    var kToggle = toggle.GetComponent<KToggle>();
                    __instance.ElementToggles.Add(tag, kToggle);
                    kToggle.group = ___toggleGroup;

                    toggle.gameObject.GetComponent<ToolTip>().toolTip = tag.ProperName();
                }
            }
        }
    }
}
