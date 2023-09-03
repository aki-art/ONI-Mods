using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitchery.Patches
{
	internal class CookingStationPatch
	{

        [HarmonyPatch(typeof(CookingStation), "OnSpawn")]
        public class CookingStation_OnSpawn_Patch
        {
            public static void Postfix(CookingStation __instance)
            {
                __instance.GetComponent<Storage>().Drop(SimHashes.Ice.CreateTag());
            }
        }
	}
}
