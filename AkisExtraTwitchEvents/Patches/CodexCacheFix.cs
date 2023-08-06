using HarmonyLib;

namespace Twitchery.Patches
{
	public class CodexCacheFix
	{
		[HarmonyPatch(typeof(CodexCache), "AddEntry")]
		public class CodexCache_AddEntry_Patch
		{
			public static bool Prefix(string id)
			{
				if (CodexCache.entries.ContainsKey(id))
				{
					Debug.LogWarning($"Tried to add {id} to the Codex screen multiple times");
					return false;
				}

				return true;
			}
		}
	}
}
