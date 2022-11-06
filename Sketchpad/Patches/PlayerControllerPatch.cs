using HarmonyLib;
using SketchPad.Tools.Pencil;
using UnityEngine;

namespace Sketchpad.Patches
{
    public class PlayerControllerPatch
    {
        [HarmonyPatch(typeof(PlayerController), "OnPrefabInit")]
        public class PlayerController_OnPrefabInit_Patch
        {
            public static void Postfix(PlayerController __instance)
            {
                //var go = new GameObject(PencilTool.NAME);
                var tool = Game.Instance.gameObject.AddComponent<PencilTool>();

                //go.transform.SetParent(__instance.gameObject.transform);

                //go.SetActive(true);
                //go.SetActive(false);

                __instance.tools = __instance.tools.AddToArray(tool);
            }
        }
    }
}
