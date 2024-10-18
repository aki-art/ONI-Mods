using HarmonyLib;

namespace Moonlet.Patches
{
	public class CodexCachePatch
	{
		[HarmonyPatch(typeof(CodexCache), "CodexCacheInit")]
		public class CodexCache_CodexCacheInit_Patch
		{
			public static void Postfix()
			{
				Mod.codexLoader.ApplyToActiveLoaders(template => template.AddToCache());
			}
		}
	}
}
