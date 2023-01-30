using HarmonyLib;
using Klei.AI;
using Twitchery.Content;

namespace Twitchery.Patches
{
    public class WorkablePatch
    {
        [HarmonyPatch(typeof(Workable), "GetEfficiencyMultiplier")]
        public class Workable_GetEfficiencyMultiplier_Patch
        {
            [HarmonyPriority(Priority.LowerThanNormal)]
            public static void Postfix(Worker worker, ref float __result)
            {
                if(worker.TryGetComponent(out Effects effects))
                {
                    if(effects.HasEffect(TEffects.CAFFEINATED))
                    {
                        __result *= 10f;
                    }
                }
            }
        }
    }
}
