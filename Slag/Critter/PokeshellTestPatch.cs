using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace Slag.Critter
{
    class PokeshellTestPatch
    {
        [HarmonyPatch(typeof(CrabConfig))]
        [HarmonyPatch("CreateCrab")]
        public static class CrabConfig_CreateCrab_Patch
        {
            public static void Postfix(ref GameObject __result)
            {

                ScaleGrowthMonitor.Def scale_growth_monitor = __result.AddOrGetDef<ScaleGrowthMonitor.Def>();
                scale_growth_monitor.defaultGrowthRate = 1f / .2f / 600f;
                scale_growth_monitor.dropMass = DreckoConfig.FIBER_PER_CYCLE * .2f;
                scale_growth_monitor.itemDroppedOnShear = DreckoConfig.EMIT_ELEMENT;
                scale_growth_monitor.levelCount = 1;
                scale_growth_monitor.targetAtmosphere = SimHashes.Oxygen;

                __result.AddTag(GameTags.Creatures.CanMolt);
            }
        }

        [HarmonyPatch(typeof(EntityTemplates))]
        [HarmonyPatch("AddCreatureBrain")]
        public static class EntityTemplates_AddCreatureBrain_Patch
        {
            public static void Prefix(GameObject prefab, ChoreTable.Builder chore_table)
            {
                if(prefab.name == CrabConfig.ID)
                {
                    chore_table.Add(new MoltStates.Def(), true);
                }
            }
        }
        
    }
}
