using CrittersDropBones.Buildings.SlowCooker;
using FUtility;
using HarmonyLib;

namespace CrittersDropBones.Patches
{
    public class ComplexFabricatorPatch
    {
        /*
        [HarmonyPatch(typeof(ComplexFabricator))]
        [HarmonyPatch("HasWorker", MethodType.Getter)]
        public class ComplexFabricator_HasWorker_Patch
        {
            public static void Postfix(ComplexFabricator __instance, ref bool __result)
            {
                if(__instance is SlowCooker cooker)
                {
                    __result = cooker.ShouldOperate;
                }
            }
        }

        [HarmonyPatch(typeof(ComplexFabricator))]
        [HarmonyPatch("Sim200ms")]
        public class ComplexFabricator_HasWorker_Patch
        {
            public static void Postfix(ComplexFabricator __instance, ref float ___orderProgress)
            {
                if (__instance is SlowCooker cooker)
                {
                    if (cooker.ShouldOperate)
                    {
                        cooker.lastOrderProgress = ___orderProgress;
                    }
                    else
                    {
                        ___orderProgress = cooker.lastOrderProgress;
                    }

                    Log.Debuglog("Progress/Last", ___orderProgress, cooker.lastOrderProgress);
                }
            }
        }
        */
    }
}
