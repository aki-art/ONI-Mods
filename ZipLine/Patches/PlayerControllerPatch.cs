using HarmonyLib;
using UnityEngine;
using ZipLine.Content.Tools;

namespace ZipLine.Patches
{
    public class PlayerControllerPatch
    {

        [HarmonyPatch(typeof(PlayerController), "OnPrefabInit")]
        public class PlayerController_OnPrefabInit_Patch
        {
            public static void Postfix(PlayerController __instance)
            {
                var go = new GameObject("Zipline Connector Tool");

                var zipConnectorTool = go.AddComponent<ZipConnectorTool>();
                zipConnectorTool.areaColour = Util.ColorFromHex(Mod.Settings.RopePathVisualizerColor);
                zipConnectorTool.invalidColor = Util.ColorFromHex(Mod.Settings.RopePathVisualizerInvalidColor);
                zipConnectorTool.maxDistance = Mod.Settings.MaxLength;
                zipConnectorTool.transform.SetParent(__instance.gameObject.transform);

                go.SetActive(true);
                go.SetActive(false);

                __instance.tools = __instance.tools.AddToArray(zipConnectorTool);
            }
        }
    }
}
