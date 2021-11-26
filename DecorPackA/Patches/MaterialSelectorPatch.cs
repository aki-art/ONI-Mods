using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace DecorPackA.Patches
{
    public class MaterialSelectorPatch
    {
        [HarmonyPatch(typeof(MaterialSelector), "ConfigureScreen")]
        public static class MaterialSelector_ConfigureScreen_Patch
        {
            public static void Postfix(MaterialSelector __instance, Recipe.Ingredient ingredient)
            {
                if (ingredient.tag == ModAssets.Tags.stainedGlassDye)
                {
                    ToggleGroup toggleGroup = Traverse.Create(__instance).Field<ToggleGroup>("toggleGroup").Value;
                    foreach (Tag tag in ModAssets.Tags.extraGlassDyes)
                    {
                        AddToggle(__instance, toggleGroup, tag);
                    }
                }

                __instance.RefreshToggleContents();
            }

            private static void AddToggle(MaterialSelector __instance, ToggleGroup ___toggleGroup, Tag tag)
            {
                if (!__instance.ElementToggles.ContainsKey(tag))
                {
                    GameObject toggle = Util.KInstantiate(__instance.TogglePrefab, __instance.LayoutContainer, "MaterialSelection_" + tag.ProperName());
                    toggle.transform.localScale = Vector3.one;
                    toggle.SetActive(true);

                    KToggle kToggle = toggle.GetComponent<KToggle>();
                    __instance.ElementToggles.Add(tag, kToggle);
                    kToggle.group = ___toggleGroup;

                    toggle.gameObject.GetComponent<ToolTip>().toolTip = tag.ProperName();

                    Element element = ElementLoader.GetElement(tag);

                    if (element is object && !element.IsSolid)
                    {
                        Image image = toggle.gameObject.GetComponentsInChildren<Image>()[1];
                        image.color = element.substance.uiColour;
                    }
                }
            }
        }
    }
}
