using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

