using HarmonyLib;
using System;
using System.Collections.Generic;
using static ProcGen.SubWorld;

namespace Beached.Patches
{
    // Credit: Heinermann (Blood mod)
    public static class EnumPatch
    {
        private static readonly Dictionary<SimHashes, string> SimHashNameLookup = new Dictionary<SimHashes, string>();
        private static readonly Dictionary<string, object> ReverseSimHashNameLookup = new Dictionary<string, object>();

        private static readonly Dictionary<ZoneType, string> ZoneTypeNameLookup = new Dictionary<ZoneType, string>();
        private static readonly Dictionary<string, object> ReverseZoneTypeNameLookup = new Dictionary<string, object>();

        public static SimHashes RegisterSimHash(string name)
        {
            var simHash = (SimHashes)Hash.SDBMLower(name);
            SimHashNameLookup.Add(simHash, name);
            ReverseSimHashNameLookup.Add(name, simHash);
            return simHash;
        }

        public static ZoneType RegisterZoneType(string name)
        {
            var zoneType = (ZoneType)Enum.GetNames(typeof(ZoneType)).Length;

            ZoneTypeNameLookup.Add(zoneType, name);
            ReverseZoneTypeNameLookup.Add(name, zoneType);
            return zoneType;
        }

        [HarmonyPatch(typeof(Enum), "GetNames", new Type[] { typeof(Type) })]
        private class Enum_GetNames_Patch
        {
            private static void Postfix(Type enumType, ref string[] __result)
            {
                if (enumType == typeof(ZoneType))
                {
                    var i = __result.Length;
                    Array.Resize(ref __result, i + ZoneTypeNameLookup.Count);
                    foreach (var zoneType in ZoneTypeNameLookup)
                    {
                        __result[i++] = zoneType.Value;
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Enum), "GetValues", new Type[] { typeof(Type) })]
        private class Enum_GetValues_Patch
        {
            private static void Postfix(Type enumType, ref Array __result)
            {
                if (enumType == typeof(ZoneType))
                {
                    var list = new List<ZoneType>(__result as ZoneType[]);
                    foreach (var zoneType in ZoneTypeNameLookup)
                    {
                        list.Add(zoneType.Key);
                    }

                    __result = list.ToArray();
                }
            }
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

                if (__instance is ZoneType type)
                {
                    return !ZoneTypeNameLookup.TryGetValue(type, out __result);
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

                if (enumType.Equals(typeof(ZoneType)))
                {
                    return !ReverseZoneTypeNameLookup.TryGetValue(value, out __result);
                }

                return true;

            }
        }
    }
}
