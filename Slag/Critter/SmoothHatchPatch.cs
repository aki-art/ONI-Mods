using Harmony;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;
using static CreatureCalorieMonitor;

namespace Slag.Critter
{
    class SmoothHatchPatch
    {
        [HarmonyPatch(typeof(Stomach), "Poop")]
        public static class Stomach_Poop_Transpiler
        {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> orig)
            {
                var codes = orig.ToList();
                var methodInfo = AccessTools.Method(typeof(Stomach_Poop_Transpiler), "PoopSlag");
                var field = AccessTools.Field(typeof(Stomach), "owner");
                codes.InsertRange(codes.Count - 2, new[]{
                                                    new CodeInstruction(OpCodes.Ldarg_0),
                                                    new CodeInstruction(OpCodes.Ldfld, field),
                                                    new CodeInstruction(OpCodes.Ldloc_0),
                                                    new CodeInstruction(OpCodes.Call, methodInfo)
                                                });

                return codes;
            }

            public static void PoopSlag(GameObject owner, float amount)
            {
                if (owner.name != HatchMetalConfig.ID) return;

                float slag_poop_mass = amount / 3f;

                int target_cell = Grid.PosToCell(owner.transform.GetPosition());
                Element element = ElementLoader.FindElementByHash(ModAssets.slagSimHash);
                float temperature = owner.GetComponent<PrimaryElement>().Temperature;

                element.substance.SpawnResource(Grid.CellToPosCCC(target_cell, Grid.SceneLayer.Ore), slag_poop_mass, temperature, 0, 0);
                PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, element.name, owner.transform, 1.5f, false);
            }
        }
    }
}
