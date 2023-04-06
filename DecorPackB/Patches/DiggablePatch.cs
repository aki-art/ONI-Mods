using DecorPackB.Content;
using DecorPackB.Content.ModDb;
using FUtility;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecorPackB.Patches
{
    internal class DiggablePatch
    {

        [HarmonyPatch(typeof(Diggable), "OnStopWork")]
        public class Diggable_OnStopWork_Patch
        {
            public static void Prefix(Diggable __instance, bool ___isDigComplete, Element ___originalDigElement)
            {
                if (___isDigComplete && __instance.worker is Worker worker)
                {
                    var cell = Grid.PosToCell(__instance);

                    if (Mod.Settings.Archeology.TreasureHunterLoots.TryGetValue(___originalDigElement.id, out var treasureTable))
                    {
                        var resume = worker.GetComponent<MinionResume>();
                        if (resume != null && resume.HasPerk(DPSkillPerks.CanFindTreasures))
                        {
                            treasureTable.OnExcavation(__instance, cell, ___originalDigElement, resume);
                        }
                    }

                    if(Mod.Settings.Archeology.BonusMaterialPercent > 0 && 
                        !Mod.isFullMinerYieldHere && 
                        (Mod.Settings.Archeology.ChanceOfBonus == 1f || UnityEngine.Random.value < Mod.Settings.Archeology.ChanceOfBonus))
                    {
                        __instance.AddTag(DPTags.DigYieldModifier);
                    }
                }
            }
        }
    }
}
