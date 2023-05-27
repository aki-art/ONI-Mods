using Bestagons.Content.Scripts;
using HarmonyLib;
using UnityEngine;

namespace Bestagons.Patches
{
    public class AsteroidConfigPatch
    {

        [HarmonyPatch(typeof(AsteroidConfig), "CreatePrefab")]
        public class AsteroidConfig_CreatePrefab_Patch
        {
            public static void Postfix(GameObject __result)
            {
                __result.AddOrGet<Bestagons_HexagonGrid>();
            }
        }
    }
}
