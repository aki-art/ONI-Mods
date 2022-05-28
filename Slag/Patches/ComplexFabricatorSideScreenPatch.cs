using FUtility;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;

namespace Slag.Patches
{
    public class ComplexFabricatorSideScreenPatch
    {
        private static readonly Sprite slagUIIcon = Def.GetUISprite("Slag").first;

        // This adds the extra tiny icons on the Metal Refinery for Slag recipes
        [HarmonyPatch(typeof(ComplexFabricatorSideScreen), "Initialize")]
        public static class ComplexFabricatorSideScreen_Initialize_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> orig)
            {
                var codes = orig.ToList();

                var m_Add = typeof(List<GameObject>).GetMethod("Add", new Type[] { typeof(GameObject) });
                var m_AddSprites = typeof(ComplexFabricatorSideScreen_Initialize_Patch).GetMethod("AddSprites", BindingFlags.NonPublic | BindingFlags.Static);

                var index = codes.FindIndex(code => code.opcode == OpCodes.Callvirt && code.operand is MethodInfo m && m == m_Add);

                if (index == -1)
                {
                    return codes;
                }

                Log.Debuglog("TRANSPILER INDEX " + index);

                codes.InsertRange(index, new[]
                {
                    // entryGO (GameObject) is loaded to stack
                    // Load recipes (Dictionary<GameObject, ComplexRecipe>) to stack
                    new CodeInstruction(OpCodes.Ldloc_S, 3),
                    // Load recipes (Dictionary<GameObject, ComplexRecipe>) to stack
                    new CodeInstruction(OpCodes.Ldloc_S, 8),
                    // Call AddSprites(entryGO, recipes, index)
                    new CodeInstruction(OpCodes.Call, m_AddSprites)
                    // puts entryGO (GameObject) back on stack
                });

                return codes;
            }

            private static GameObject AddSprites(GameObject entryGO, ComplexRecipe[] recipes, int index)
            {
                Log.Debuglog("PATCH");
                Log.Debuglog(index);
                Log.Debuglog(recipes.GetType().ToString());
                if (entryGO == null || recipes == null)
                {
                    return entryGO;
                }

                var recipe = recipes[index];

                if (recipe.nameDisplay == ModAssets.slagNameDisplay)
                {
                    var img = entryGO.GetComponentsInChildrenOnly<Image>();
                    if (img != null && img.Length >= 3)
                    {
                        var slagIcon = UnityEngine.Object.Instantiate(img[2], img[2].transform);
                        slagIcon.rectTransform.localScale = Vector3.one * .8f;
                        slagIcon.sprite = slagUIIcon;
                    }
                }

                return entryGO;
            }
        }
    }
}
