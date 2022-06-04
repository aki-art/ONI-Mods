using FUtility;
using HarmonyLib;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TransparentAluminum.Content;

namespace TransparentAluminum.Patches
{
    public class ElementLoaderPatch
    {
        public static Hashtable substanceList;

        [HarmonyPatch(typeof(ElementLoader), "CollectElementsFromYAML")]
        public class ElementLoader_CollectElementsFromYAML_Patch
        {
            public static void Postfix(ref List<ElementLoader.ElementEntry> __result)
            {
                var path = Path.Combine(Utils.ModPath, "assets", "elements.yaml");

                ElementsBase.ReadYAML(path, ref __result);
                Elements.RegisterSubstances(substanceList);
            }
        }
    }

    [HarmonyPatch(typeof(ElementLoader), "Load")]
    public class ElementLoader_Load_Patch
    {
        public static void Prefix(ref Hashtable substanceList)
        {
            ElementLoaderPatch.substanceList = substanceList;
        }

        public static void Postfix()
        {
            Elements.SetSolidMaterials();
        }
    }
}
