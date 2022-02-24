using FUtility;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;

namespace Beached.Patches
{
    public class CodexCachePatch
    {
        [HarmonyPatch(typeof(CodexCache), "CollectEntries")]
        public static class CodexCache_CollectEntries_Patch
        {
            public static void Postfix(string folder, List<CodexEntry> __result)
            {
                if (folder == "")
                {
                    var extraEntries = CodexCache.CollectEntries(Path.Combine(Utils.ModPath, "codex", "Creatures"));
                    __result.AddRange(extraEntries);
                }
            }
        }

        [HarmonyPatch(typeof(CodexCache), "CollectSubEntries")]
        public static class CodexCache_CollectSubEntries_Patch
        {
            public static void Postfix(string folder, List<SubEntry> __result)
            {
                if (folder == "")
                {
                    var extraEntries = CodexCache.CollectSubEntries(Path.Combine(Utils.ModPath, "codex", "Creatures"));
                    __result.AddRange(extraEntries);
                }
            }
        }
    }
}
