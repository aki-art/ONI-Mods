using HarmonyLib;
using PrintingPodRecharge.Cmps;
using UnityEngine;

namespace PrintingPodRecharge.Patches
{
    internal class MinionConfigPatch
    {

        [HarmonyPatch(typeof(MinionConfig), "CreatePrefab")]
        public class MinionConfig_CreatePrefab_Patch
        {
            public static void Postfix(GameObject __result)
            {
                __result.AddOrGet<CustomDupe>();
            }
        }
    }
}
