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

                Element element = ElementLoader.GetElement(SimHashes.Fossil.CreateTag());

                if (element is null)
                {
                    return;
                }

                // initialize if it doesn't exist yet
                if (element.oreTags is null)
                {
                    element.oreTags = new Tag[] { };
                }

                // add my tag
                element.oreTags = element.oreTags.AddToArray(ModAssets.Tags.Fossil);
            }
        }
    }
}
