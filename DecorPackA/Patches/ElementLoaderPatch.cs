using FUtility;
using HarmonyLib;
using System.Collections.Generic;

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
                foreach(var entry in ModAssets.tiles) {
                    Log.Debuglog("Adding tag ", entry.Key);
                    var element = ElementLoader.FindElementByName(entry.Key.ToString());

                    if (!(element is object)) continue;

                    // initialize if it doesn't exist yet
                    if (element.oreTags.IsNullOrDestroyed())
                    {
                        element.oreTags = new Tag[] { };
                    }

                    // add my tag
                    element.oreTags = element.oreTags.AddToArray(ModAssets.Tags.stainedGlassDye);
                    Log.Debuglog(string.Join(", ", element.oreTags));
                }
            }
        }
    }
}
