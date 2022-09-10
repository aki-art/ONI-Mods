using HarmonyLib;
using PrintingPodRecharge.Cmps;

namespace PrintingPodRecharge.Patches
{
    public class WorkerPatch
    {
        //[HarmonyPatch(typeof(Worker), "AttachOverrideAnims")]
        public class Worker_AttachOverrideAnims_Patch
        {
            public static void Postfix(Worker __instance, KAnimControllerBase worker_controller)
            {
                if(__instance != null)
                {
                    HairDye.Apply(__instance, worker_controller as KBatchedAnimController);
                }
            }
        }
    }
}
