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
            // probably should be in a transpiler instead before the RefreshToggleContents call in the original method
            public static void Postfix(MaterialSelector __instance, Recipe.Ingredient ingredient, ToggleGroup ___toggleGroup)
            {
                if (ingredient.tag == ModAssets.Tags.stainedGlassDye)
                {
                    foreach (Tag tag in ModAssets.Tags.extraGlassDyes)
                    {
                        AddToggle(__instance, ___toggleGroup, tag);
                    }

                    __instance.RefreshToggleContents();
                }
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
                }
            }
        }
    }
}
