using HarmonyLib;
using System;
using System.Collections.Generic;

namespace Slag.Patches
{
    // Credit: Heinermann (Blood mod)
    public static class EnumPatch
    {
        private static readonly Dictionary<SimHashes, string> SimHashNameLookup = new Dictionary<SimHashes, string>();
        private static readonly Dictionary<string, object> ReverseSimHashNameLookup = new Dictionary<string, object>();

        public static SimHashes RegisterSimHash(string name)
        {
            var simHash = (SimHashes)Hash.SDBMLower(name);
            SimHashNameLookup.Add(simHash, name);
            ReverseSimHashNameLookup.Add(name, simHash);
            return simHash;
        }

        [HarmonyPatch(typeof(Enum), "ToString", new Type[] { })]
        private class SimHashes_ToString_Patch
        {
            private static bool Prefix(ref Enum __instance, ref string __result)
            {
                if (__instance is SimHashes hashes)
                {
                    return !SimHashNameLookup.TryGetValue(hashes, out __result);
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
                    return !ReverseSimHashNameLookup.TryGetValue(value, out __result);
                }

                return true;
            }
        }
    }
}
