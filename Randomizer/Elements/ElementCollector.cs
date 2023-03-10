using System;
using System.Collections.Generic;
using System.Linq;

namespace Randomizer.Elements
{
    public class ElementCollector
    {
        public const string
            METAL1 = "METAL1",
            METAL2 = "METAL2",
            MINERAL1 = "MINERAL1",
            MINERAL2 = "MINERAL2",
            LIQUID = "LIQUID",
            GAS = "GAS",
            PRECIOUS = "PRECIOUS",
            SOLIDS = "SOLIDS";

        public ElementCollection metalOres;
        public ElementCollection waters;
        public ElementCollection minerals;
        public ElementCollection breathables;
        public ElementCollection farmables;
        public ElementCollection gases;
        public ElementCollection liquids;
        public ElementCollection solids;
        public ElementCollection all;
        public ElementCollection irons;

        public List<ElementCollection> elementCollections;

        public class DIG_HARDNESS
        {
            public const byte
                SOFT = 50,
                MEDIUM = 150,
                HARD = 200,
                SUPERHARD = 251,
                UNDIGGABLE = 255;
        }

        public void Collect()
        {
            var availableElements = ElementLoader.elements
                .Where(e => DlcManager.IsContentActive(e.dlcId))
                .Where(e => !e.name.StartsWith("MISSING."))
                .ToList();

            CreateCollections();

            foreach (var element in availableElements)
            {
                foreach (var collection in elementCollections)
                {
                    if (collection.filter(element))
                    {
                        collection.Add(element);
                    }
                }
            }
        }

        private void CreateCollections()
        {
            elementCollections = new();

            var ironSimHashes = new HashSet<SimHashes>()
            {
                SimHashes.Iron,
                SimHashes.IronGas,
                SimHashes.IronOre,
                SimHashes.MoltenIron,
                SimHashes.FoolsGold
            };

            metalOres = Add(e => e.HasTag(GameTags.Ore) && e.HasTag(GameTags.Metal));
            waters = Add(e => e.HasTag(GameTags.Water) && e.IsLiquid && e.lowTemp < 283.15f && e.highTemp > 333.15f);
            breathables = Add(e => e.HasTag(GameTags.Breathable));
            farmables = Add(e => e.HasTag(GameTags.Farmable));
            minerals = Add(e => e.HasTag(GameTags.BuildableRaw));
            gases = Add(e => e.IsGas);
            liquids = Add(e => e.IsLiquid);
            solids = Add(e => e.IsSolid);
            all = Add(e => true);
            irons = Add(e => ironSimHashes.Contains(e.id));
        }

        private ElementCollection Add(Func<Element, bool> filter)
        {
            var collection = new ElementCollection(filter);
            elementCollections.Add(collection);

            return collection;
        }
    }
}
