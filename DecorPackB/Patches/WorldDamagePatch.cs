using DecorPackB.Content;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace DecorPackB.Patches
{
    public class WorldDamagePatch
    {
        [HarmonyPatch(typeof(WorldDamage), "OnDigComplete")]
        public class FullMinerYield_WorldDamage_OnDigComplete
        {
            private static void Prefix(int cell, ref float mass)
            {
                mass *= 1f + Mod.Settings.Archeology.BonusMaterialPercent;
                mass *= 2f; // it will be halved
            }

            /*
            private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var codes = instructions.ToList();
                var m_AdjustMass = typeof(FullMinerYield_WorldDamage_OnDigComplete).GetMethod("AdjustMass", BindingFlags.NonPublic | BindingFlags.Static);

                codes.InsertRange(codes.Count - 1, new[]
                {
                    new CodeInstruction(OpCodes.Ldloc_3),
                    new CodeInstruction(OpCodes.Ldloc_2),
                    new CodeInstruction(OpCodes.Ldarg_2),
                    new CodeInstruction(OpCodes.Call, m_AdjustMass)
                });

                return codes;
            }
            */
            private static void AdjustMass(GameObject go, float spawnedMass, float originalMass)
            {
                if(go == null || !go.HasTag(DPTags.DigYieldModifier))
                {
                    return;
                }

                var mass = spawnedMass * (1f + Mod.Settings.Archeology.BonusMaterialPercent);
                mass = Mathf.Clamp(mass, 0, originalMass);

                go.GetComponent<PrimaryElement>().Mass = mass;
            }
        }
    }
}
