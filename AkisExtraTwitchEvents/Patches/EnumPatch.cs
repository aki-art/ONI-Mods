using HarmonyLib;
using System;
using Twitchery.Utils;

namespace Twitchery.Patches
{
	// Credit: Heinermann (Blood mod)
	public static class EnumPatch
	{
		[HarmonyPatch(typeof(Enum), "ToString", new Type[] { })]
		public class SimHashes_ToString_Patch
		{
			public static bool Prefix(ref Enum __instance, ref string __result)
			{
				if (__instance is SimHashes hashes)
					return !ElementUtil.SimHashNameLookup.TryGetValue(hashes, out __result);

				return true;
			}
		}

		[HarmonyPatch(typeof(Enum), nameof(Enum.Parse), new Type[] { typeof(Type), typeof(string), typeof(bool) })]
		public class SimHashes_Parse_Patch
		{
			public static bool Prefix(Type enumType, string value, ref object __result)
			{
				if (enumType.Equals(typeof(SimHashes)))
					return !ElementUtil.ReverseSimHashNameLookup.TryGetValue(value, out __result);

				return true;
			}
		}
	}
}
