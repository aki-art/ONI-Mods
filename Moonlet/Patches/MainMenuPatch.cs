using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonlet.Patches
{
	internal class MainMenuPatch
	{

        [HarmonyPatch(typeof(MainMenu), "OnPrefabInit")]
        public class MainMenu_OnPrefabInit_Patch
        {
            public static void Postfix(MainMenu __instance)
            {
                var moonletWatermark = UnityEngine.Object.Instantiate(__instance.buildWatermark);

            }
        }
	}
}
