using HarmonyLib;

namespace DecorPackB.Patches
{
    public class ElementLoaderPatch
    {
        [HarmonyPatch(typeof(ElementLoader), "Load")]
        public static class Patch_ElementLoader_Load
        {
            public static void Postfix()
            {
                Mod.Colors.ProcessColors(Mod.Colors);
            }
        }
    }
}
