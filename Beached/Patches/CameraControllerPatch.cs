using HarmonyLib;

namespace Beached.Patches
{
    internal class CameraControllerPatch
    {
        [HarmonyPatch(typeof(CameraController))]
        [HarmonyPatch("OnPrefabInit")]
        public static class CameraController_OnPrefabInit_Patch
        {

        }
    }
}

