using HarmonyLib;
using Twitchery.Content.Scripts;

namespace Twitchery.Patches
{
	public class ZoneTilePatch
	{
		[HarmonyPatch(typeof(ZoneTile), "OnSpawn")]
		public class ZoneTile_OnSpawn_Patch
		{
			public static void Postfix(ZoneTile __instance)
			{
				AkisTwitchEvents.Instance.AddZoneTile(__instance);
			}
		}

		[HarmonyPatch(typeof(ZoneTile), "ClearZone")]
		public class ZoneTile_ClearZone_Patch
		{
			public static void Postfix(ZoneTile __instance)
			{
				AkisTwitchEvents.Instance.RemoveZoneTile(__instance);
			}
		}
	}
}
