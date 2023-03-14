using DecorPackA;
using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DecorPackA.Patches
{
    public class MaterialSelectorPatch
    {
        public const float Y_OFFSET = 48f;

        // colors liquid & gas icons
        [HarmonyPatch(typeof(MaterialSelector), "SetToggleBGImage")]
        public class MaterialSelector_SetToggleBGImage_Patch
        {
            public static void Postfix(KToggle toggle, Tag elem, Recipe.Ingredient ___activeIngredient)
            {
                try
                {
                    ReplaceSprite(toggle, elem, ___activeIngredient);
                }
                catch (Exception e) when (e is NullReferenceException)
                {
                    Log.Warning($"Something went wrong: {elem} {___activeIngredient?.tag}");
                    Log.Assert("toggle", toggle);
                    Log.Assert("elem", elem);
                    Log.Assert("___activeIngredient", ___activeIngredient);
                    Log.Assert("___activeIngredient.tag", ___activeIngredient.tag);
                }
            }

            private static void ReplaceSprite(KToggle toggle, Tag elem, Recipe.Ingredient ___activeIngredient)
            {
                if (___activeIngredient == null ||
                    toggle == null ||
                    ___activeIngredient.tag != ModAssets.Tags.stainedGlassDye)
                {
                    return;
                }

                var elementSprite = toggle.gameObject.GetComponentsInChildren<Image>()[1];

                if (elementSprite == null)
                {
                    Log.Warning("element sprite is null, cannot color it.");
                    return;
                }

                if (ElementLoader.GetElement(elem) is Element element && !element.IsSolid)
                {
                    var prefab = Assets.GetPrefab(element.tag);
                    if (prefab != null)
                    {
                        var tuple = Def.GetUISprite(prefab);
                        if (tuple != null)
                        {
                            elementSprite.sprite = tuple.first;
                        }
                    }

                    elementSprite.color = element.substance.uiColour;
                }
            }
        }

        [HarmonyPatch(typeof(MaterialSelector), "UpdateScrollBar")]
        public class MaterialSelector_UpdateScrollBar_Patch
        {
            public static void Postfix(MaterialSelector __instance, Recipe.Ingredient ___activeIngredient)
            {
                if (___activeIngredient.tag != ModAssets.Tags.stainedGlassDye)
                {
                    return;
                }

                __instance.ScrollRect.GetComponent<LayoutElement>().minHeight = 74 * 4;
            }
        }

        // forces liquid and gas options without enabling them for other buildings
        [HarmonyPatch(typeof(MaterialSelector), "ConfigureScreen")]
        public static class MaterialSelector_ConfigureScreen_Patch
        {
            public static void Postfix(MaterialSelector __instance, Recipe.Ingredient ingredient, ToggleGroup ___toggleGroup)
            {
                Log.Assert("ingredient", ingredient);

                var gridLayoutGroup = __instance.GetComponentInChildren<GridLayoutGroup>();

                if (gridLayoutGroup != null)
                {
                    if (ingredient.tag != ModAssets.Tags.stainedGlassDye)
                    {
                        __instance.GetComponentInChildren<GridLayoutGroup>().cellSize = new Vector2(48, 70);
                        return;
                    }

                    __instance.GetComponentInChildren<GridLayoutGroup>().cellSize = new Vector2(48, 70 + Y_OFFSET);
                }

                if (ingredient.tag == ModAssets.Tags.stainedGlassDye)
                {
                    foreach (var tag in ModAssets.Tags.extraGlassDyes)
                    {
                        AddToggle(__instance, ___toggleGroup, tag);
                    }
                }

                __instance.RefreshToggleContents();

                if (gridLayoutGroup == null)
                {
                    return;
                }

                foreach (var toggle in __instance.ElementToggles)
                {
                    if (toggle.Value.gameObject == null)
                    {
                        continue;
                    }

                    var elementSprite = toggle.Value.gameObject.GetComponentsInChildren<Image>()[1];
                    if (elementSprite == null)
                    {
                        Log.Warning($"element sprite of {toggle.Key} is null in MaterialSelector_ConfigureScreen_Patch");
                        continue;
                    }
                    var secondSprite = Util.KInstantiate(elementSprite, elementSprite.transform.parent.gameObject);

                    var id = Mod.PREFIX + toggle.Key + "StainedGlassTile";

                    var buildingDef = Assets.GetBuildingDef(id);
                    if (buildingDef == null)
                    {
                        Log.Warning($"buildingDef {id} doesn't exist / MaterialSelector_ConfigureScreen_Patch");
                        continue;
                    }

                    if (secondSprite.TryGetComponent(out Image image))
                    {
                        image.sprite = buildingDef.GetUISprite();
                        image.color = Color.white;
                    }

                    var rectTransform = secondSprite.AddOrGet<RectTransform>();
                    rectTransform.localPosition = new Vector2(-2f, -18);
                    rectTransform.localScale = new Vector2(1.4f, 1.4f);

                    var materialCounter = toggle.Value.GetComponentsInChildren<LocText>()?[1];
                    if (materialCounter != null)
                    {
                        var counterRect = materialCounter.gameObject.AddOrGet<RectTransform>();
                        counterRect.localPosition += new Vector3(0, -Y_OFFSET);
                    }
                }
            }

            private static void AddToggle(MaterialSelector __instance, ToggleGroup ___toggleGroup, Tag tag)
            {
                if (__instance.ElementToggles != null &&
                    !__instance.ElementToggles.ContainsKey(tag))
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
