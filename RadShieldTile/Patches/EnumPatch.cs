using FUtility.ElementUtil;
using HarmonyLib;
using System;

namespace RadShieldTile.Patches
{
	// Credit: Heinermann (Blood mod)
	public static class EnumPatch
	{
		[HarmonyPatch(typeof(Enum), "ToString", new Type[] { })]
		private class SimHashes_ToString_Patch
		{
			private static bool Prefix(ref Enum __instance, ref string __result)
			{
				if (__instance is SimHashes hashes)
				{
					return !ElementUtil.SimHashNameLookup.TryGetValue(hashes, out __result);
				}

				return true;
			}
		}

		[HarmonyPatch(typeof(Enum), nameof(Enum.Parse), [typeof(Type), typeof(string), typeof(bool)])]
		private class SimHashes_Parse_Patch
		{
			private static bool Prefix(Type enumType, string value, ref object __result)
			{
				if (enumType.Equals(typeof(SimHashes)))
				{
					return !ElementUtil.ReverseSimHashNameLookup.TryGetValue(value, out __result);
				}

				return true;
			}
		}
	}
}
