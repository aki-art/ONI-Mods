using HarmonyLib;
using Twitchery.Content;

namespace Twitchery.Patches
{
	public class MinionStartingStatsPatch
	{
		[HarmonyPatch(typeof(MinionStartingStats), nameof(MinionStartingStats.CreateBodyData))]
		public class MinionStartingStats_CreateBodyData_Patch
		{
			public static void Postfix(Personality p, ref KCompBuilder.BodyData __result)
			{
				TPersonalities.ModifyBodyData(p, ref __result);
			}
		}

		[HarmonyPatch(typeof(MinionStartingStats), nameof(MinionStartingStats.GenerateTraits))]
		public class MinionStartingStats_GenerateTraits_Patch
		{
			public static void Prefix(MinionStartingStats __instance)
			{
				TPersonalities.OnTraitRoll(__instance);
			}
		}
	}
}
