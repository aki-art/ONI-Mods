using HarmonyLib;
using System.Collections.Generic;
using Twitchery.Content;

namespace Twitchery.Patches
{
	public class CodexEntryGeneratorPatch
    {
        [HarmonyPatch(typeof(CodexEntryGenerator), "GenerateFoodEntries")]
        public class CodexEntryGenerator_GenerateFoodEntries_Patch
        {
            public static void Postfix(ref Dictionary<string, CodexEntry> __result)
            {
                __result.Remove(Elements.Jello.ToString());
            }
        }

        [HarmonyPatch(typeof(CodexCache), "AddEntry")]
        public class CodexCache_AddEntry_Patch
        {
            public static bool Prefix(string id, CodexEntry entry)
            {
                // skip adding to foods it would be a duplicate entry with element)
                return entry.parentId != CodexEntryGenerator.FOOD_CATEGORY_ID || id != Elements.Jello.ToString();
            }
        }
    }
}
