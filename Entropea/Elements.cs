using Entropea.Gen;
using FUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Entropea.Gen.BiomeGenerator;
using static Entropea.Gen.SubWorldsGenerator.SubWorldInfo;

namespace Entropea
{
    class Elements
    {
        static Element defaultElement;
        public static Dictionary<State, List<Element>> elementsByState;
        public static List<Element> whiteListedElements = new List<Element>();
        public static List<Element> metals = new List<Element>();
        public static List<SimHashes> blacklistedElements = new List<SimHashes>()
        {
            SimHashes.Vacuum,
            SimHashes.Void,
            SimHashes.Unobtanium,
            SimHashes.Creature
        };

        public static void Initialize()
        {
            defaultElement = ElementLoader.FindElementByHash(SimHashes.SandStone);
            whiteListedElements = ElementLoader.elements.FindAll(e => !blacklistedElements.Contains(e.id));
            metals = ElementLoader.elements.FindAll(e => e.IsSolid && e.HasTag(GameTags.Metal) && !blacklistedElements.Contains(e.id));

            elementsByState = new Dictionary<State, List<Element>>
            {
                [State.Any] = whiteListedElements,
                [State.Solid] = GetElementsWithState(Element.State.Solid),
                [State.Liquid] = GetElementsWithState(Element.State.Liquid),
                [State.Gas] = GetElementsWithState(Element.State.Gas),
                [State.SolidOrLiquid] = GetElementsWithState(Element.State.Solid, Element.State.Liquid),
                [State.LiquidOrGas] = GetElementsWithState(Element.State.Solid, Element.State.Liquid),
                [State.GasOrSolid] = GetElementsWithState(Element.State.Solid, Element.State.Liquid)
            };

            blacklistedElements = ReadElementList("blacklistedElements");
        }

        public static SimHashes GetRandomElement(State state, float temperature, byte hardness = 255, bool allowUnstable = true, List<SimHashes> blackList = null)
        {
            var possibleElements = elementsByState[state];
            if (blackList == null) blackList = new List<SimHashes>();

            possibleElements = possibleElements
                .FindAll(e =>
                    blackList.Contains(e.id) &&
                    e.lowTemp <= temperature &&
                    e.highTemp >= temperature &&
                    e.hardness <= hardness &&
                    !(!allowUnstable && e.IsUnstable));


            if (possibleElements.Count == 0)
                return SimHashes.SandStone;

            return possibleElements.GetRandom().id;
        }
        internal static SimHashes GetRandomElement(State solid, float specificTemperature, byte digLevel, bool allowUnstable, Dictionary<ElementRole, SimHashes> elements)
        {
            List<SimHashes> blacklist = elements.Select(d => d.Value).ToList();
            return GetRandomElement(solid, specificTemperature, digLevel, allowUnstable, elements);
        }

        private static List<SimHashes> ReadElementList(string fileName, bool weighted = false)
        {
            List<string> list = new List<string>();
            string filePath = Path.Combine(ModAssets.ModPath, "config", fileName + ".json");

            try
            {
                using (var r = new StreamReader(filePath))
                {
                    var json = r.ReadToEnd();
                    list = JsonConvert.DeserializeObject<List<string>>(json);
                }
            }
            catch (Exception e)
            {
                Log.Warning($"Couldn't read {filePath}, {e.Message}.");
                return null;
            }

            List<SimHashes> result = new List<SimHashes>();
            foreach(var element in list)
            {
                if (Enum.TryParse(element, out SimHashes simHash))
                    result.Add(simHash);
            }

            return result;
        }

        private static List<Element> GetElementsWithState(Element.State state, Element.State state2 = Element.State.Vacuum)
        {
            List<Element> result = whiteListedElements.FindAll(e => e.state == state);

            if (state2 != Element.State.Vacuum)
                result.Union(whiteListedElements.FindAll(e => e.state == state2));

            return result;
        }

        public enum State
        {
            Solid,
            Liquid,
            Gas,
            Any,
            SolidOrLiquid,
            LiquidOrGas,
            GasOrSolid
        }

        public static class DigLevel
        {
            public static byte Soft = 10;
            public static byte Firm = 25;
            public static byte VeryFirm = 50;
            public static byte NearlyImpenetrable = 150;
            public static byte Impenetrable = 255;
        }


    }
}
