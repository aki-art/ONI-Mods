using HarmonyLib;
using System;

namespace TransparentAluminum.Patches
{
    public class EnumPatches
    {
        [HarmonyPatch(typeof(Enum), "ToString", new Type[] { })]
        private class SimHashes_ToString_Patch
        {
            private static bool Prefix(ref Enum __instance, ref string __result)
            {
                if (__instance is SimHashes hashes)
                {
                    return FUtility.ElementsBase.TryGetName(hashes, out __result);
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(Enum), nameof(Enum.Parse), new Type[] { typeof(Type), typeof(string), typeof(bool) })]
        private class SimHashes_Parse_Patch
        {
            private static bool Prefix(Type enumType, string value, ref object __result)
            {
                if (enumType.Equals(typeof(SimHashes)))
                {
                    return FUtility.ElementsBase.TryGetSimHash(value, out __result);
                }

                return true;
            }
        }
    }
}
