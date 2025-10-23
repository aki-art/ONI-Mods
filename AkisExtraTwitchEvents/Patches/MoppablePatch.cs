using HarmonyLib;

namespace Twitchery.Patches
{
	public class MoppablePatch
	{
		[HarmonyPatch(typeof(Moppable), "OnSpawn")]
		public class Moppable_OnSpawn_Patch
		{
			public static void Postfix(Moppable __instance)
			{
				Mod.moppables.Add(__instance);
			}
		}

		[HarmonyPatch(typeof(Moppable), "OnCleanUp")]
		public class Moppable_OnCleanUp_Patch
		{
			public static void Postfix(Moppable __instance)
			{
				Mod.moppables.Remove(__instance);
			}
		}
	}
}
