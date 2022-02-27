using Beached.Germs;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.Patches
{
    internal class ColorSetPatches
    {
        [HarmonyPatch(typeof(ColorSet), "Init")]
        public static class ColorSet_Init_Patch
        {
            public static void Postfix(Dictionary<string, Color32> ___namedLookup)
            {
                if(!___namedLookup.ContainsKey(PlanktonGerms.ID))
                {
                    ___namedLookup.Add(PlanktonGerms.ID, ModAssets.Colors.plankton);
                }
            }
        }
    }
}
