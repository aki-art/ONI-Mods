﻿using Backwalls.Cmps;
using HarmonyLib;
using UnityEngine;

namespace Backwalls.Patches
{
    internal class WorldPatch
    {

        [HarmonyPatch(typeof(World), "OnPrefabInit")]
        public class World_OnPrefabInit_Patch
        {
            public static void Postfix(World __instance)
            {
                Mod.renderer = __instance.gameObject.AddOrGet<BackwallRenderer>();
            }
        }
    }
}
