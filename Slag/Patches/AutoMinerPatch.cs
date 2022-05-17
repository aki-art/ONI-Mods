using FUtility;
using HarmonyLib;
using Slag.Content.Critters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Slag.Patches
{
    internal class AutoMinerPatch
    {
        /*
        [HarmonyPatch(typeof(AutoMiner), "DigBlockingCB")]
        public class AutoMiner_DigBlockingCB_Patch
        {
            public static void Postfix(int cell, ref bool __result)
            {
                if (!__result)
                {
                    var collision_entries = ListPool<ScenePartitionerEntry, SelectTool>.Allocate();
                    GameScenePartitioner.Instance.GatherEntries(Extents.OneCell(cell), GameScenePartitioner.Instance.pickupablesLayer, collision_entries);

                    foreach (var entry in collision_entries)
                    {
                        if (entry.obj is Pickupable go)
                        {
                            Log.Debuglog(go.name);
                        }

                        if (entry.obj is Pickupable obj && obj.TryGetComponent(out MineableCreature mineable) && mineable.IsMineable())
                        {
                            Log.Debuglog("MINEABLE CB");
                            __result = true;
                            return;
                        }
                    }

                    collision_entries.Recycle();
                }

                Log.Debuglog("tried cb");
            }
        }*/

        [HarmonyPatch(typeof(AutoMiner), "ValidDigCell")]
        public class AutoMiner_ValidDigCell_Patch
        {

            public static void Postfix(int cell, ref bool __result)
            {
                if(!__result)
                {
                    var collision_entries = ListPool<ScenePartitionerEntry, SelectTool>.Allocate();
                    GameScenePartitioner.Instance.GatherEntries(Extents.OneCell(cell), GameScenePartitioner.Instance.pickupablesLayer, collision_entries);

                    foreach (var entry in collision_entries)
                    {
                        Log.Debuglog("FOUND OBJECT");

                        if (entry.obj is Pickupable obj && obj.TryGetComponent(out MineableCreature mineable) && mineable.IsMineable())
                        {
                            Log.Debuglog("MINEABLE");
                            __result = true;
                            return;
                        }
                    }

                    collision_entries.Recycle();
                }

                Log.Debuglog("tried");
            }
        }

        /*
        [HarmonyPatch(typeof(AutoMiner), "RefreshDiggableCell")]
        public class AutoMiner_RefreshDiggableCell_Patch
        {
            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> orig)
            {
                var codes = orig.ToList();

                var m_Add = typeof(List<GameObject>).GetMethod("Add", new Type[] { typeof(GameObject) });

                for (int i = 0; i < codes.Count; i++)
                {
                    if(codes[i].opcode == OpCodes.Call && 
                }
                var m_Add = typeof(List<GameObject>).GetMethod("Add", new Type[] { typeof(GameObject) });
                var m_AddSprites = typeof(ComplexFabricatorSideScreen_Initialize_Patch).GetMethod("AddSprites", BindingFlags.NonPublic | BindingFlags.Static);

                var index = codes.FindIndex(code => code.opcode == OpCodes.Callvirt && code.operand is MethodInfo m && m == m_Add);

                if (index == -1)
                {
                    return codes;
                }

                codes.InsertRange(index, new[]
                {
                    // entryGO (GameObject) is loaded to stack
                    // Load recipes (ComplexRecipe[]) to stack
                    new CodeInstruction(OpCodes.Ldloc_S, 4),
                    // Load index (int) to stack
                    new CodeInstruction(OpCodes.Ldloc_S, 5),
                    // Call AddSprites(entryGO, recipes, index)
                    new CodeInstruction(OpCodes.Call, m_AddSprites)
                    // puts entryGO (GameObject) back on stack
                });

                return codes;
            }
        }
        */
    }
}
