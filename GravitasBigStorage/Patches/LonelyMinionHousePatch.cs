using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitasBigStorage.Patches
{
    internal class LonelyMinionHousePatch
    {

        [HarmonyPatch(typeof(LonelyMinionHouse.Instance), "CompleteEvent")]
        public class LonelyMinionHouse_Instance_CompleteEvent_Patch
        {
            public static void Postfix()
            {
            }
        }
    }
}
