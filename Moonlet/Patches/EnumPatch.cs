using HarmonyLib;
using Moonlet.Utils;
using System;
using static ProcGen.SubWorld;

namespace Moonlet.Patches
{
	public static class EnumPatch
	{
		public class SimHashes_Parse_Patch
		{
			public static void Patch(Harmony harmony)
			{
				var m_Parse = AccessTools.Method(typeof(Enum), nameof(Enum.Parse), [typeof(Type), typeof(string), typeof(bool)]);
				var prefix = AccessTools.Method(typeof(SimHashes_Parse_Patch), nameof(Prefix));

				harmony.Patch(m_Parse, new HarmonyMethod(prefix));
			}

			public static bool Prefix(Type enumType, string value, ref object __result)
			{
				if (enumType.Equals(typeof(SimHashes)))
					return !ElementUtil.ReverseSimHashNameLookup.TryGetValue(value, out __result);

				if (enumType.Equals(typeof(ZoneType)))
					return !ZoneTypeUtil.TryParse(value, out __result);

				return true;
			}
		}

		public class SimHashes_ToString_Patch
		{
			public static void Patch(Harmony harmony)
			{
				var m_ToString = AccessTools.Method(typeof(Enum), "ToString", []);
				var prefix = AccessTools.Method(typeof(SimHashes_ToString_Patch), "Prefix");

				harmony.Patch(m_ToString, new HarmonyMethod(prefix));
			}

			public static bool Prefix(ref Enum __instance, ref string __result)
			{
				if (__instance is SimHashes hashes)
					return !ElementUtil.SimHashNameLookup.TryGetValue(hashes, out __result);

				return true;
			}
		}

	}
}
