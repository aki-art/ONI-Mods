using DecorPackA.Buildings;
using HarmonyLib;
using UnityEngine;

namespace DecorPackA.Patches
{
    public class FlowerVaseHangingFancyConfigPatch
    {
        [HarmonyPatch(typeof(FlowerVaseHangingFancyConfig), "ConfigureBuildingTemplate")]
        public class FlowerVaseHangingFancyConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix(GameObject go)
            {
                go.AddOrGet<FacadeRestorer>();
            }
        }
    }
}
