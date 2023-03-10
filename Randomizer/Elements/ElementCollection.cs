using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer.Elements
{
    public class ElementCollection
    {
        private readonly List<Element> elements;

        public Func<Element, bool> filter;

        public ElementCollection(Func<Element, bool> filter)
        {
            this.filter = filter;
            elements = new();
        }

        public void Add(Element element)
        {
            elements.Add(element);
        }

        public List<Element> GetAllStartBiomeAppropiate()
        {
            return elements
                .Where(IsStartBiomeAppropiate)
                .ToList();
        }

        public Element GetRandomStartBiomeAppropiate()
        {
            elements.Shuffle();
            return elements.Find(IsStartBiomeAppropiate);
        }

        public Element Get(float low, float high, float maxHardness = 0)
        {
            elements.Shuffle();
            return elements.Find(e =>
                !e.IsSolid || e.hardness <= maxHardness &&
                IsBetweenTemps(e, low, high));
        }

        public Element Get(float low, float high, float minHardness, float maxHardness)
        {
            elements.Shuffle();
            return elements.Find(e =>
                !e.IsSolid || (e.hardness <= maxHardness && e.hardness >= minHardness) &&
                IsBetweenTemps(e, low, high));
        }

        private static bool IsStartBiomeAppropiate(Element e)
        {
            return
                IsBetweenTemps(e, 283.15f, 333.15f) &&
                e.hardness <= ElementCollector.DIG_HARDNESS.SOFT;
        }

        private static bool IsBetweenTemps(Element e, float low, float high)
        {
            return
                e.lowTemp < low &&
                e.highTemp > high;
        }
    }
}
