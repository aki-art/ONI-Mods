/*using HarmonyLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Patches
{
    public class DivergentWormConfigPatch
    {
        [HarmonyPatch(typeof(DivergentWormConfig), "CreateWorm")]
        public class DivergentWormConfig_CreateWorm_Patch
        {
            public static void Postfix(GameObject __result)
            {
                __result.AddOrGet<LongWormy>();
            }
        }
    }
}
*/