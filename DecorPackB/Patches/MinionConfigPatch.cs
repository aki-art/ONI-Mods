using DecorPackB.Content.ModDb;
using DecorPackB.Content.Scripts;
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
                __result.AddOrGet<ArcheologistRestorer>().skillId = DPSkills.ARCHEOLOGY_ID;
            }
        }
    }
}
