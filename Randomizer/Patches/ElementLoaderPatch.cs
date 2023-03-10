using HarmonyLib;

namespace Randomizer.Patches
{
    public class ElementLoaderPatch
    {
        [HarmonyPatch(typeof(ElementLoader), "Load")]
        public class ElementLoader_Load_Patch
        {
            public static void Postfix()
            {
                Mod.elementCollector.Collect();
            }
        }
    }
}
