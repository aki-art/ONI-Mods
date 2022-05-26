using DecorPackA.Buildings.StainedGlassTile;
using HarmonyLib;

namespace DecorPackA.Patches
{
    internal class CodexCachePatch
    {

        [HarmonyPatch(typeof(CodexCache), "CodexCacheInit")]
        public class CodexCache_CodexCacheInit_Patch
        {
            public static void Postfix()
            {
                PaletteCodexEntry.GeneratePaletteEntry2();
            }
        }
    }
}
