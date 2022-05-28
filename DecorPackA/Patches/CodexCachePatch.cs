using DecorPackA.Buildings.StainedGlassTile;
using HarmonyLib;
using System.Collections.Generic;

namespace DecorPackA.Patches
{
    internal class CodexCachePatch
    {
        public const string MODS = "MODS";

        [HarmonyPatch(typeof(CodexCache), "CodexCacheInit")]
        public class CodexCache_CodexCacheInit_Patch
        {
            public static void Postfix()
            {
                PaletteCodexEntry.GeneratePaletteEntry();
                CreateModEntry(new CodexEntry(MODS, new List<ContentContainer>(), "title"));
            }
        }

        public static Dictionary<string, CodexEntry> CreateModEntry(CodexEntry entry)
        {
            var results = new Dictionary<string, CodexEntry>()
            {
                { PaletteCodexEntry.PALETTE, entry }
            };

            CreateModsCategory(MODS, results);

            return results;
        }

        // Multiple mods can do this with the same category ID, they would merge
        private static void CreateModsCategory(string ID, Dictionary<string, CodexEntry> entries)
        {
            var categoryEntry = new CodexEntry(ID, new List<ContentContainer>(), STRINGS.UI.CODEX.CATEGORYNAMES.MODS)
            {
                id = ID,
                category = ""
            };

            if (!CodexCache.entries.ContainsKey(ID))
            {
                CodexCache.AddEntry(ID, categoryEntry, null);
            }
            else
            {
                CodexCache.MergeEntry(ID, categoryEntry);
            }
        }
    }
}
