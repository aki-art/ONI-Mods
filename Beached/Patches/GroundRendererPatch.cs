using System;
using System.Collections.Generic;
using System.Linq;
using FUtility;
using HarmonyLib;
using ProcGen;

namespace Beached.Patches
{
    internal class GroundRendererPatch
    {
        [HarmonyPatch(typeof(GroundRenderer), "OnPrefabInit")]
        public static class GroundRenderer_OnPrefabInit_Patch
        {
            public static void Postfix(GroundRenderer __instance, ref GroundMasks.BiomeMaskData[] ___biomeMasks)
            {
                int beachIndex = (int)ModAssets.ZoneTypes.beach;
                int depthsIndex = (int)ModAssets.ZoneTypes.depths;

                Array.Resize(ref ___biomeMasks, ___biomeMasks.Length + 1);
                ___biomeMasks[beachIndex] = ___biomeMasks.FirstOrDefault(b => b.name == "boggymarsh");
                ___biomeMasks[depthsIndex] = ___biomeMasks.FirstOrDefault(b => b.name == "boggymarsh");

                foreach (var mask in ___biomeMasks)
                {
                    Log.Debuglog("MASKNAME " + mask?.name);
                }

                Traverse
                    .Create(___biomeMasks[beachIndex])
                    .Method("Regenerate")
                    .GetValue();


                Traverse
                    .Create(___biomeMasks[depthsIndex])
                    .Method("Regenerate")
                    .GetValue();
            }
        }
    }
}
