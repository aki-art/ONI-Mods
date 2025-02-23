using HarmonyLib;
using Moonlet.Scripts;

namespace Moonlet.Patches
{
	public class ZoneTilePatch
	{
		[HarmonyPatch(typeof(ZoneTile), "OnSpawn")]
		public class ZoneTile_OnSpawn_Patch
		{
			public static void Postfix(ZoneTile __instance)
			{
				Moonlet_Mod.Instance.AddZoneTile(__instance);
			}
		}

		[HarmonyPatch(typeof(ZoneTile), "ClearZone")]
		public class ZoneTile_ClearZone_Patch
		{
			public static void Postfix(ZoneTile __instance)
			{
				Moonlet_Mod.Instance.RemoveZoneTile(__instance);
			}
		}
	}
}
