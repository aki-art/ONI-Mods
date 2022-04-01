using HarmonyLib;
using PeterHan.PLib.Core;

namespace GoldenThrone.Patches
{
    public class FlushToiletConfigPatch
    {
        [HarmonyPatch(typeof(FlushToiletConfig), "CreateBuildingDef")]
        public class FlushToiletConfig_CreateBuildingDef_Patch
        {
            public static void Postfix()
            {
                PGameUtils.CopySoundsToAnim("gold_flush_kanim", "toiletflush_kanim");
            }
        }
    }
}
