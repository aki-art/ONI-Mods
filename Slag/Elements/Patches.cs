using Harmony;
using Klei;
using Klei.AI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Slag.Elements
{
    class Patches
    {
        private static readonly string assetDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets");


        internal static Hashtable subList;
        internal static Material solidMaterial;

        [HarmonyPatch(typeof(LegacyModMain), "ConfigElements")]
        class LegacyModMain_ConfigElements_Patch
        {
            private static void Postfix()
            {
                Element slag = ElementLoader.FindElementByHash(ModAssets.slagSimHash);
                slag.attributeModifiers.Add(new AttributeModifier(Db.Get().BuildingAttributes.OverheatTemperature.Id, 120, slag.name));

                Element slagGlass = ElementLoader.FindElementByHash(ModAssets.slagGlassSimHash);
                slagGlass.attributeModifiers.Add(new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, .15f, slagGlass.name, true));
            }
        }

        [HarmonyPatch(typeof(ElementLoader), "CollectElementsFromYAML")]
        class ElementLoader_CollectElementsFromYAML_Patch
        {
            private static void Postfix(ref List<ElementLoader.ElementEntry> __result)
            {
                Strings.Add("STRINGS.ELEMENTS.SLAG.NAME", STRINGS.UI.FormatAsLink("Slag", "SLAG"));
                Strings.Add("STRINGS.ELEMENTS.SLAG.DESC", $"Description of Slag");
                Strings.Add("STRINGS.ELEMENTS.SLAGGLASS.NAME", STRINGS.UI.FormatAsLink("Slag Glass", "SLAGGLASS"));
                Strings.Add("STRINGS.ELEMENTS.SLAGGLASS.DESC", $"Description of Slag Glass");

                string elementListText = File.ReadAllText(Path.Combine(assetDirectory, "elements.txt"));
                ElementLoader.ElementEntryCollection elementList = YamlIO.Parse<ElementLoader.ElementEntryCollection>(elementListText, new FileHandle());

                __result.AddRange(elementList.elements);

                subList.Add(ModAssets.slagSimHash, Substances.SlagElement(solidMaterial));
                subList.Add(ModAssets.slagGlassSimHash, Substances.SlagGlassElement(solidMaterial));
            }
        }

        [HarmonyPatch(typeof(ElementLoader), "Load")]
        private static class Patch_ElementLoader_Load
        {
            private static void Prefix(ref Hashtable substanceList, SubstanceTable substanceTable)
            {
                subList = substanceList;
                solidMaterial = substanceTable.solidMaterial;
            }
        }
    }
}