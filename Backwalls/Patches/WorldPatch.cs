using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backwalls.Patches
{
    internal class WorldPatch
    {

        [HarmonyPatch(typeof(World), "OnPrefabInit")]
        public class World_OnPrefabInit_Patch
        {
            public static void Postfix(World __instance)
            {
                Mod.renderer = __instance.gameObject.AddOrGet<BackwallRenderer2>();
            }
        }
    }
}
