using FUtility;
using HarmonyLib;
using Slag.Content;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace Slag.Patches
{
    public class CreatureCalorieMonitorStomachPatch
    {
        [HarmonyPatch(typeof(CreatureCalorieMonitor.Stomach), "Poop")]
        public static class CreatureCalorieMonitor_Stomach_Poop_Transpiler
        {
            private static readonly Vector3 offset = new Vector3(0.25f, 0);

            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> orig)
            {
                var codes = orig.ToList();

                var m_PoopSlag = typeof(CreatureCalorieMonitor_Stomach_Poop_Transpiler).GetMethod("PoopSlag", BindingFlags.Static | BindingFlags.NonPublic);

                Log.Debuglog("is method null?");
                Log.Debuglog(m_PoopSlag == null);
                var m_SpawnFX = typeof(PopFXManager).GetMethod("SpawnFX", new[] { typeof(Sprite), typeof(string), typeof(Transform), typeof(float), typeof(bool) });
                //var f_owner = typeof(CreatureCalorieMonitor.Stomach).GetField("owner", BindingFlags.NonPublic | BindingFlags.Instance);
                var f_owner = AccessTools.Field(typeof(CreatureCalorieMonitor.Stomach), "owner");

                var index = codes.FindIndex(code => code.operand is MethodInfo m && m == m_SpawnFX);

                if (index == -1)
                {
                    return codes;
                }

                codes.InsertRange(index + 1, new[]
                {
                    // Load instance (CreatureCalorieMonitor.Stomach)
                    new CodeInstruction(OpCodes.Ldarg_0),
                    // Load owner (GameObject)
                    new CodeInstruction(OpCodes.Ldfld, f_owner),
                    // Load originalPoopMass (float)
                    new CodeInstruction(OpCodes.Ldloc_0),
                    // call PoopSlag(owner, originalPoopMass)
                    new CodeInstruction(OpCodes.Call, m_PoopSlag)
                });

                return codes;
            }

            private static void PoopSlag(GameObject owner, float originalPoopMass)
            {
                if (owner == null || owner.PrefabID() != HatchMetalConfig.ID)
                {
                    return;
                }

                var mass = originalPoopMass / 3f;

                var targetCell = Grid.PosToCell(owner.transform.GetPosition());
                var element = ElementLoader.FindElementByHash(Elements.Slag);
                var temperature = owner.GetComponent<PrimaryElement>().Temperature;

                element.substance.SpawnResource(Grid.CellToPosCCC(targetCell, Grid.SceneLayer.Ore) + offset, mass, temperature, 0, 0);
                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, element.name, owner.transform);
            }
        }
    }
}
