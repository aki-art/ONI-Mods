﻿using HarmonyLib;
using SpookyPumpkinSO;

namespace SpookyPumpkinSO.Patches
{
    public class DbPatch
    {
        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                ModAssets.LateLoadAssets();
            }
        }
    }
}
