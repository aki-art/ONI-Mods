using FUtility;
using HarmonyLib;
using Klei;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Beached.Patches
{
    public class ElementLoaderPatch
    {
        internal static Hashtable substanceList;

        [HarmonyPatch(typeof(ElementLoader), "CollectElementsFromYAML")]
        public class ElementLoader_CollectElementsFromYAML_Patch
        {
            public static void Postfix(ref List<ElementLoader.ElementEntry> __result)
            {
                var path = Path.Combine(Utils.ModPath, "assets", "elements", "elements.yaml");
                var elementListText = ReadText(path);

                if (elementListText.IsNullOrWhiteSpace())
                {
                    return;
                }

                var elementList = YamlIO.Parse<ElementLoader.ElementEntryCollection>(elementListText, new FileHandle());
                __result.AddRange(elementList.elements);

                Elements.RegisterSubstances(substanceList);
            }

            private static string ReadText(string path)
            {
                try
                {
                    return File.ReadAllText(path);
                }
                catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
                {
                    Log.Warning($"Element configuration could not be read: {e.Message}\n" +
                        $"Transparent Aluminium will not be added to the game.");

                    return null;
                }
            }
        }


        [HarmonyPatch(typeof(ElementLoader), "Load")]
        public class ElementLoader_Load_Patch
        {
            public static void Prefix(ref Hashtable substanceList, Dictionary<string, SubstanceTable> substanceTablesByDlc)
            {
                ElementLoaderPatch.substanceList = substanceList;
            }

            public static void Postfix()
            {
                Elements.SetSolidMaterials();
            }
        }
    }
}
