using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace WorldCreep.SandBox
{
    class SandboxToolsPatches
    {

        [HarmonyPatch(typeof(ToolMenu), "CreateSandBoxTools")]
        public static class ToolMenu_CreateSandBoxTools_Patch
        {
            public static void Postfix()
            {
                ToolMenu.ToolCollection seismicTool = ToolMenu.CreateToolCollection(
                    "Seismic Event", 
                    "brush", 
                    Action.NumActions, 
                    "SeismicEventSpawnerTool", 
                    "rumbly stuff", 
                    false);

                ToolMenu.Instance.sandboxTools.Add(seismicTool);
            }
        }

        [HarmonyPatch(typeof(PlayerController), "OnPrefabInit")]
        public static class PlayerController_OnPrefabInit_Patch
        {
            public static void Postfix(PlayerController __instance)
            {
                List<InterfaceTool> interfaceTools = new List<InterfaceTool>(__instance.tools);

                GameObject seismicTool = new GameObject("SeismicEventSpawnerTool", typeof(SeismicEventSpawnerTool));
                seismicTool.transform.SetParent(__instance.gameObject.transform);
                seismicTool.gameObject.SetActive(true);
                seismicTool.gameObject.SetActive(false);

                interfaceTools.Add(seismicTool.GetComponent<InterfaceTool>());
                __instance.tools = interfaceTools.ToArray();
            }
        }
    }
}
