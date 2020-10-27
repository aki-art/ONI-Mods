using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;

namespace TransparentAluminium
{
    public class GlassTilePatches
    {
        // TODO: find index properly
        // replaces the building tool to build transparent aluminium glass instead of regular glass
        [HarmonyPatch(typeof(PlanScreen))]
        [HarmonyPatch("OnRecipeElementsFullySelected")]
        public static class PlanScreen_OnRecipeElementsFullySelected_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(ILGenerator generator, IEnumerable<CodeInstruction> orig)
            {
                var ShouldReplace = AccessTools.Method(typeof(PlanScreen_OnRecipeElementsFullySelected_Patch), "ShouldReplace", new Type[] { typeof(string), typeof(string) });
                var NewDef = AccessTools.Method(typeof(Assets), "GetBuildingDef", new Type[] { typeof(string) });

                var dontchange = generator.DefineLabel();
                var startLabel = generator.DefineLabel();

                int index = 27;

                var codes = orig.ToList();
                codes[index].labels.Add(dontchange);
                codes[index + 1].labels.Add(startLabel);
                codes.Insert(index++, new CodeInstruction(OpCodes.Ldstr, GlassTileConfig.ID));
                codes.Insert(index++, new CodeInstruction(OpCodes.Ldstr, ModAssets.TransparentAluminum.ToString()));
                codes.Insert(index++, new CodeInstruction(OpCodes.Call, ShouldReplace)); 
                codes.Insert(index++, new CodeInstruction(OpCodes.Brfalse, dontchange));
                codes.Insert(index++, new CodeInstruction(OpCodes.Ldstr, TransparentAluminiumTileConfig.ID));
                codes.Insert(index++, new CodeInstruction(OpCodes.Callvirt, NewDef));
                codes.Insert(index++, new CodeInstruction(OpCodes.Stloc_0));
                codes.Insert(index++, new CodeInstruction(OpCodes.Br, startLabel));
                return codes;
            }

            private static bool ShouldReplace(string target, string element)
            {
                BuildingDef def = CurrentDef();
                return def != null && def.name == target && IsMyElementSelected(element);
            }

            private static BuildingDef CurrentDef()
            {
                KToggle currentlySelectedToggle = Traverse.Create(PlanScreen.Instance).Field("currentlySelectedToggle").GetValue<KToggle>();
                return PlanScreen.Instance.ActiveToggles.FirstOrDefault(t => t.Value == currentlySelectedToggle).Key;
            }

            private static bool IsMyElementSelected(Tag element)
            {
                ProductInfoScreen productInfoScreen = Traverse.Create(PlanScreen.Instance).Field("productInfoScreen").GetValue<ProductInfoScreen>();
                return productInfoScreen.materialSelectionPanel.GetSelectedElementAsList.Contains(element);
            }
        }

        // TODO: transpiler
        // Makes the Copy Building button target default glass in the build menu.
        [HarmonyPatch(typeof(PlanScreen), "OnClickCopyBuilding")]
        public static class PlanScreen_OnClickCopyBuilding_Patch
        {
            public static void Prefix(GameObject ___copyBuildingButton)
            {
                KSelectable selectable = SelectTool.Instance.selected;
                if (selectable != null)
                {
                    Building building = SelectTool.Instance.selected.GetComponent<Building>();

                    if (building != null && building.Def.name == TransparentAluminiumTileConfig.ID)
                    {
                        var buildingDefault = UnityEngine.Object.Instantiate(building);
                        buildingDefault.Def = Assets.GetBuildingDef(GlassTileConfig.ID);
                        if (buildingDefault != null)
                        {
                            PlanScreen.Instance.CopyBuildingOrder(buildingDefault);

                            buildingDefault.gameObject.SetActive(false);
                            UnityEngine.Object.Destroy(buildingDefault);

                            ___copyBuildingButton.SetActive(false);
                        }
                    }
                }
            }
        }
    }
}
