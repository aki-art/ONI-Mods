using HarmonyLib;
using System.Collections.Generic;

namespace Twitchery.Patches
{
	public class CodexEntryGeneratorPatch
	{
		public static HashSet<string> edibleElement = new HashSet<string>()
		{
			"Jello",
			"FrozenHoney",
			"Ice"
		};

		[HarmonyPatch(typeof(CodexEntryGenerator), "GenerateFoodEntries")]
		public class CodexEntryGenerator_GenerateFoodEntries_Patch
		{
			public static void Postfix(ref Dictionary<string, CodexEntry> __result)
			{
				foreach (var element in edibleElement)
					__result.Remove(element);
			}
		}

		[HarmonyPatch(typeof(CodexCache), "AddEntry")]
		public class CodexCache_AddEntry_Patch
		{
			public static bool Prefix(string id, CodexEntry entry)
			{
				// skip adding to foods it would be a duplicate entry with element)
				return entry.parentId != CodexEntryGenerator.FOOD_CATEGORY_ID || !edibleElement.Contains(id);
			}
		}
	}
}
