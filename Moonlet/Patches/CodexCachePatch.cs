using HarmonyLib;

namespace Moonlet.Patches
{
	public class CodexCachePatch
	{
		// this is called in the middle of CodexCache.Init, but before it post processes the entries
		[HarmonyPatch(typeof(CodexCache), "CheckUnlockableContent")]
		public class CodexCache_CheckUnlockableContent_Patch
		{
			public static void Postfix()
			{
				Mod.codexLoader.ApplyToActiveLoaders(template => template.AddToCache());
			}
		}
	}
}
