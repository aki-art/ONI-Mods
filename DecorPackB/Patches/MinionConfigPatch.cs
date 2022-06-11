using DecorPackB.Cmps;
using HarmonyLib;
using UnityEngine;

namespace DecorPackB.Patches
{
    public class MinionConfigPatch
    {
        [HarmonyPatch(typeof(MinionConfig), "CreatePrefab")]
        public class MinionConfig_CreatePrefab_Patch
        {
            public static void Postfix(GameObject __result)
            {
                __result.AddOrGet<ArcheologistRestorer>().skillId = ModDb.Skills.ARCHEOLOGY_ID;
            }
        }
    }
}
