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
                    if(Mod.treasureChances.TryGetValue(___originalDigElement.id, out var treasureTable))
                    {
                        var resume = worker.GetComponent<MinionResume>();
                        if (resume != null && resume.HasPerk(ModAssets.SkillPerks.CanFindTreasures))
                        {
                            treasureTable.OnExcavation(__instance, ___originalDigElement, resume);
                        }
                    }
                }
            }
        }
    }
}
