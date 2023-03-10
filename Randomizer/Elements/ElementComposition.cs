using System.Collections.Generic;

namespace Randomizer.Elements
{
    public class ElementComposition
    {
        public Dictionary<string, Element> elements;

        private readonly float minTemperature;
        private readonly float maxTemperature;
        private readonly float maxHardness;

        public Element primary; 
        public Element secondary; 
        public Element tertiary; 

        public ElementComposition(float minTemperature, float maxTemperature, float maxHardness, bool start, float borderMinHardness, float borderMaxHardness)
        {
            this.minTemperature = minTemperature;
            this.maxTemperature = maxTemperature;
            this.maxHardness = maxHardness;

            elements = new()
            {
                { ElementRole.METAL1, Get(Mod.elementCollector.metalOres) },
                { ElementRole.METAL2, Get(Mod.elementCollector.metalOres) },
                { ElementRole.MINERAL1, Get(Mod.elementCollector.minerals) },
                { ElementRole.MINERAL2, Get(Mod.elementCollector.minerals) },
                { ElementRole.LIQUID, Get(start ? Mod.elementCollector.waters : Mod.elementCollector.liquids) },
                { ElementRole.GAS, Get(start ? Mod.elementCollector.breathables : Mod.elementCollector.gases) },
                { ElementRole.FARMABLE, Get( Mod.elementCollector.farmables) },
                { ElementRole.BORDER1, GetBorder( Mod.elementCollector.solids, borderMinHardness, borderMaxHardness) },
                { ElementRole.BORDER2, GetBorder( Mod.elementCollector.solids, borderMinHardness, borderMaxHardness) },
            };
        }

        public Element GetRandom(string role)
        {
            if (elements.TryGetValue(role, out var element))
            {
                return element;
            }

            return null;
        }

        private Element Get(ElementCollection collection)
        {
            return collection.Get(minTemperature, maxTemperature, maxHardness);
        }

        private Element GetBorder(ElementCollection collection, float minHardness, float maxHardness)
        {
            return collection.Get(minTemperature, maxTemperature, minHardness, maxHardness);
        }
    }
}
