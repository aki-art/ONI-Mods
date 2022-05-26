using DecorPackA.Buildings.StainedGlassTile;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA.Patches
{
    public class CodexScreenPatch
    {
        [HarmonyPatch(typeof(CodexScreen), "SetupPrefabs")]
        public class CodexScreen_SetupPrefabs_Patch
        {
            public static void Postfix(Dictionary<Type, GameObject> ___ContentPrefabs, GameObject ___prefabLabelWithLargeIcon)
            {
                ___ContentPrefabs[typeof(StainedGlassCodexLabel)] = ___prefabLabelWithLargeIcon;
            }
        }
    }
}
