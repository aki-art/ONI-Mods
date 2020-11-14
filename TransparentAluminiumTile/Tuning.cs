using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using TransparentAluminium.SolarPanelRoads;

namespace TransparentAluminium
{
    public class Tuning
    {
        private const float defaultSolarMaxWatt = 50f;
        private const float defaultSolarWattPerLux = 0.00053f / 4;
        private const float solarMultiplier = 1.5f;
        private static float GetLeveledValue(float val, float level) => Mathf.Pow(solarMultiplier, level) * val;
        public static float GetLeveledMaxWatt(float level) => GetLeveledValue(defaultSolarMaxWatt, level);
        public static float GetLeveledWPL(float level) => GetLeveledValue(defaultSolarWattPerLux, level);

        public struct Upgrade
        {
            public float mass;
            public Tag tag;

            public Upgrade(Tag tag, float mass)
            {
                this.mass = mass;
                this.tag = tag;
            }
        }

        Dictionary<Tag, Upgrade[]> upgrades = new Dictionary<Tag, Upgrade[]>()
        {
            {
                SolarRoad1Config.ID, new Upgrade[]
                {
                    new Upgrade(SimHashes.Glass.CreateTag(), 100),
                    new Upgrade(SimHashes.Iron.CreateTag(), 100)
                }
            },
            { 
                SolarRoad2Config.ID, new Upgrade[]
                {
                    new Upgrade(SimHashes.Diamond.CreateTag(), 200),
                    new Upgrade(SimHashes.Iron.CreateTag(), 400),
                    new Upgrade(SimHashes.Ceramic.CreateTag(), 400)
                }
            },
            {
                SolarRoad3Config.ID, new Upgrade[]
                {
                    new Upgrade(ModAssets.TransparentAluminum, 200),
                    new Upgrade(SimHashes.Gold.CreateTag(), 600),
                    new Upgrade(SimHashes.Ceramic.CreateTag(), 1000)
                }
            },
            {
                SolarRoad4Config.ID, new Upgrade[]
                {
                    new Upgrade(ModAssets.TransparentAluminum, 500),
                    new Upgrade(SimHashes.Niobium.CreateTag(), 400),
                    new Upgrade(SimHashes.Fullerene.CreateTag(), 100)
                }
            }
        };

        public static CellOffset[] solarCellOffsets = new CellOffset[14]
        {
                new CellOffset(-3, 2),
                new CellOffset(-2, 2),
                new CellOffset(-1, 2),
                new CellOffset(0, 2),
                new CellOffset(1, 2),
                new CellOffset(2, 2),
                new CellOffset(3, 2),
                new CellOffset(-3, 1),
                new CellOffset(-2, 1),
                new CellOffset(-1, 1),
                new CellOffset(0, 1),
                new CellOffset(1, 1),
                new CellOffset(2, 1),
                new CellOffset(3, 1)
        };


    }
}
