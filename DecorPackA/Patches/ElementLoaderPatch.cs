using DecorPackA.Buildings.StainedGlassTile;
using HarmonyLib;

namespace DecorPackA.Patches
{
    class ElementLoaderPatch
    {
        // add tags to some elements
        [HarmonyPatch(typeof(ElementLoader), "Load")]
        public static class Patch_ElementLoader_Load
        {
            public static void Postfix()
            {
                foreach (var entry in StainedGlassTiles.tileInfos)
                {
                    var element = ElementLoader.GetElement(entry.ElementTag);

                    if (element is null) continue;

                    // initialize if it doesn't exist yet
                    if (element.oreTags is null)
                    {
                        element.oreTags = new Tag[] { };
                    }

                    // add my tag
                    element.oreTags = element.oreTags.AddToArray(ModAssets.Tags.stainedGlassDye);
                }

                Element blood = ElementLoader.GetElement("Blood");
                if (!(blood is null))
                {
                    ModAssets.Tags.extraGlassDyes.Add("Blood");
                }
            }
        }
    }
}
