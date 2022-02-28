using FUtility;
using HarmonyLib;
using System;
using System.Linq;

namespace Beached.Patches
{
    internal class GroundRendererPatch
    {
        [HarmonyPatch(typeof(GroundRenderer), "OnPrefabInit")]
        public static class GroundRenderer_OnPrefabInit_Patch
        {
            public static void Postfix(GroundRenderer __instance, ref GroundMasks.BiomeMaskData[] ___biomeMasks)
            {
                var beachIndex = (int)ModAssets.ZoneTypes.beach;
                var depthsIndex = (int)ModAssets.ZoneTypes.depths;
                var bambooIndex = (int)ModAssets.ZoneTypes.bamboo;

                Array.Resize(ref ___biomeMasks, ___biomeMasks.Length + 1);
                var reference = ___biomeMasks.FirstOrDefault(b => b.name == "boggymarsh");

                ___biomeMasks[beachIndex] = reference;
                ___biomeMasks[depthsIndex] = reference;
                ___biomeMasks[bambooIndex] = reference;

                foreach (var mask in ___biomeMasks)
                {
                    Log.Debuglog("MASKNAME " + mask?.name);
                }

                Traverse.Create(___biomeMasks[beachIndex]).Method("Regenerate").GetValue();
                Traverse.Create(___biomeMasks[depthsIndex]).Method("Regenerate").GetValue();
                Traverse.Create(___biomeMasks[bambooIndex]).Method("Regenerate").GetValue();
            }
        }
    }
}
