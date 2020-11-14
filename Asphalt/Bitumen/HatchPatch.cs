using Harmony;
using System.Collections.Generic;

namespace Asphalt.Bitumen
{
    public class HatchPatch
    {
        [HarmonyPatch(typeof(BaseHatchConfig), "BasicRockDiet")]
        public static class HatchConfig_BasicRockDiet_Patch
        {
            public static void Postfix(List<Diet.Info> __result) => __result[0].consumedTags.Add(SimHashes.Bitumen.CreateTag());
        }
    }
}
