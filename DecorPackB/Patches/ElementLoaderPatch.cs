using DecorPackB.Content;
using HarmonyLib;
using UnityEngine;

namespace DecorPackB.Patches
{
    public class ElementLoaderPatch
    {
        [HarmonyPatch(typeof(ElementLoader), "Load")]
        public static class Patch_ElementLoader_Load
        {
            public static void Postfix()
            {
                var fossil = ElementLoader.FindElementByHash(SimHashes.Fossil);
                var permafrost = ElementLoader.FindElementByName("Permafrost");

                TryAddTag(fossil, DPTags.liteFossilMaterial);
                TryAddTag(fossil, DPTags.trueFossilMaterial);
                TryAddTag(permafrost, DPTags.liteFossilMaterial);
                TryAddTag(permafrost, DPTags.trueFossilMaterial);
                TryAddTag(ElementLoader.FindElementByHash(SimHashes.Lime), DPTags.liteFossilMaterial);
                TryAddTag(ElementLoader.FindElementByName("Bone"), DPTags.liteFossilMaterial);
            }

            private static void TryAddTag(Element element, Tag tag)
            {
                if(element == null)
                {
                    return;
                }

                element.oreTags = element.oreTags == null ? new Tag[] { tag } : element.oreTags.AddToArray(tag);
            }
        }
    }
}
