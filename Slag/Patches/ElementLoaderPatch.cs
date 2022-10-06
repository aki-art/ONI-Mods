using FUtility;
using HarmonyLib;
using Klei;
using Slag.Content;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Slag.Patches
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

                if (Mod.Settings.RegolithToSlagMeltingRatio > 0)
                {
                    foreach (var element in __result)
                    {
                        if (element.elementId == SimHashes.Regolith.ToString())
                        {
                            element.highTempTransitionOreId = Elements.Slag.ToString();
                            element.highTempTransitionOreMassConversion = Mod.Settings.RegolithToSlagMeltingRatio;
                        }
                    }
                }

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
                        $"Slag will not be added to the game. :(");

                    return null;
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

            // This second postfix runs a little later, to make sure any element adding mods have settled
            [HarmonyPostfix]
            [HarmonyPriority(Priority.Low)]
            public static void PostfixLate()
            {
                Mod.gleamiteRewards.Settings.SanitizeRewards();
                Mod.slagmiteRewards.Settings.SanitizeRewards();
            }
        }
    }
}
