using Database;
using HarmonyLib;
using Slag.Content.Buildings.InsulatedWindowTile;
using System.Collections.Generic;

namespace Slag.Patches
{
    public class TechsPatch
    {
        // add tech to database
        [HarmonyPatch(typeof(Techs), "Init")]
        public class Techs_TargetMethod_Patch
        {
            public static void Postfix(Techs __instance)
            {
                new Tech(ModAssets.Techs.ADVANCED_INSULATION_ID, new List<string>
                {
                    InsulatedWindowTileConfig.ID
                },
                __instance);
            }
        }

        /* add tech to research screen
        [HarmonyPatch(typeof(Techs), "Load")]
        public class Techs_Load_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> orig)
            {
                var codes = orig.ToList();

                var m_AddNode = typeof(Techs_Load_Patch).GetMethod("AddNode", BindingFlags.Static | BindingFlags.NonPublic);

                // right after the local nodes list was created
                var index = codes.FindIndex(c => c.opcode == OpCodes.Stloc_0);

                if(index == -1)
                {
                    return codes;
                }

                codes.InsertRange(index + 1, new[] {
                    // load local variable 0 to stack (tech_tree_nodes)
                    new CodeInstruction(OpCodes.Ldloc_0),
                    // call AddNode(tech_tree_nodes)
                    new CodeInstruction(OpCodes.Call, m_AddNode)
                });

                return codes;
            }
        }
        */
    }
}
