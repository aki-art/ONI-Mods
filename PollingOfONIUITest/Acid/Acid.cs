using Harmony;
using Klei;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PollingOfONIUITest.Acid
{
    public class Acid
    {
        public static readonly SimHashes AcidSimHash = (SimHashes)Hash.SDBMLower("Acid");

        [HarmonyPatch(typeof(ElementLoader), "CollectElementsFromYAML")]
        public class ElementLoader_CollectElementsFromYAML
        {
            public static void Postfix(ref List<ElementLoader.ElementEntry> __result)
            {
                Strings.Add("STRINGS.ELEMENTS.ACID.NAME", STRINGS.UI.FormatAsLink("Acid", "ACID"));
                Strings.Add("STRINGS.ELEMENTS.ACID.DESC", $"Refreshing Sprite.");

                var elementCollection = YamlIO.Parse<ElementLoader.ElementEntryCollection>(ACIDTEST, new FileHandle());
                __result.AddRange(elementCollection.elements);
            }
        }

        [HarmonyPatch(typeof(ElementLoader), "Load")]
        public class ElementLoader_Load
        {
            public static void Prefix(ref Hashtable substanceList, SubstanceTable substanceTable)
            {
                Color32 green = new Color32(173, 245, 66, 255);
                Substance water = substanceTable.GetSubstance(SimHashes.Water);
                substanceList[AcidSimHash] = ModUtil.CreateSubstance(
                    name: "Acid",
                    state: Element.State.Liquid,
                    kanim: water.anim,
                    material: water.material,
                    colour: green,
                    ui_colour: green,
                    conduit_colour: green
                  );
            }
        }

        public const string ACIDTEST = @"
---
elements:
  - elementId: Acid
    maxMass: 1000
    liquidCompression: 1.02
    speed: 100
    minHorizontalFlow: 0.1
    minVerticalFlow: 0.01
    specificHeatCapacity: 3.49
    thermalConductivity: 0.58
    solidSurfaceAreaMultiplier: 1
    liquidSurfaceAreaMultiplier: 25
    gasSurfaceAreaMultiplier: 1
    lowTemp: 272.5
    highTemp: 372.5
    lowTempTransitionTarget: Dirt
    highTempTransitionTarget: Steam
    highTempTransitionOreId: Iron
    highTempTransitionOreMassConversion: 0.001
    defaultTemperature: 310
    defaultMass: 1000
    molarMass: 255
    toxicity: 1
    lightAbsorptionFactor: 0.8
    tags:
    - AnyWater
    isDisabled: false
    state: Liquid
    localizationID: STRINGS.ELEMENTS.ACID.NAME
";
    }
}
