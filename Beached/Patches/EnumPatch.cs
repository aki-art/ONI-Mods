using HarmonyLib;
using System;
using System.Collections.Generic;

namespace Beached.Patches
{
    // Credit: Heinermann (Blood mod)
    public static class EnumPatch
    {
        public static Dictionary<SimHashes, string> SimHashNameLookup = new Dictionary<SimHashes, string>();
        public static readonly Dictionary<string, object> ReverseSimHashNameLookup = new Dictionary<string, object>();

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
                if (!(__instance is SimHashes))
                {
                    return true;
                }

                return !SimHashNameLookup.TryGetValue((SimHashes)__instance, out __result);
            }
        }

        [HarmonyPatch(typeof(Enum), nameof(Enum.Parse), new Type[] { typeof(Type), typeof(string), typeof(bool) })]
        private class SimHashes_Parse_Patch
        {
            private static bool Prefix(Type enumType, string value, ref object __result)
            {
                if (!enumType.Equals(typeof(SimHashes)))
                {
                    return true;
                }

                return !ReverseSimHashNameLookup.TryGetValue(value, out __result);
            }
        }
    }
}
