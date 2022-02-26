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

                Log.Debuglog("GroundRenderer ------------------------- ");

                Array.Resize(ref ___biomeMasks, ___biomeMasks.Length + 1);
                ___biomeMasks[beachIndex] = ___biomeMasks.FirstOrDefault(b => b.name == "boggymarsh");

                /*
                var GetBiomeMaskMethod = Traverse
                    .Create(__instance)
                    .Method("GetBiomeMask", new Type[] { typeof(SubWorld.ZoneType) });
                */
                //___biomeMasks[beachIndex] = GetBiomeMaskMethod.GetValue<GroundMasks.BiomeMaskData>(ModAssets.ZoneTypes.beach);

                foreach (var mask in ___biomeMasks)
                {
                    Log.Debuglog("MASKNAME " + mask?.name);
                }

                Traverse
                    .Create(___biomeMasks[beachIndex])
                    .Method("Regenerate")
                    .GetValue();
            }
        }
    }
}
