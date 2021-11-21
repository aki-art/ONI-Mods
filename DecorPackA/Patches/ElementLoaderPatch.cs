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
                foreach (StainedGlassTiles.TileInfo entry in StainedGlassTiles.tileInfos)
                {
                    Element element = ElementLoader.GetElement(entry.ElementTag);

                    if (element is null) continue;

                    // initialize if it doesn't exist yet
                    if (element.oreTags is null)
                    {
                        element.oreTags = new Tag[] { };
                    }

                    if (!entry.solid)
                    {
                        ModAssets.Tags.extraGlassDyes.Add(entry.ElementTag.ToString());
                    }

                    // add my tag
                    element.oreTags = element.oreTags.AddToArray(ModAssets.Tags.stainedGlassDye);
                }
            }
        }
    }
}
