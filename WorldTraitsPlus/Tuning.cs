﻿using ProcGen;
using System.Collections.Generic;
using UnityEngine;

namespace WorldTraitsPlus
{
    public class Tuning
    {
        public class Colors
        {
            public static Color32 seismicOverlayActive = new Color32(183, 112, 219, 255);
            public static Color32 seismicDispersionArea = new Color32(247, 155, 35, 255);
            public static Color32 seismicEpicenter = new Color32(255, 0, 0, 255);
            public static Color32 seismicVignette = new Color(1, 0.55f, 0.05f, 1);
        }

        public class FXHashes
        {
            public static SpawnFXHashes MegaMeteorImpact = (SpawnFXHashes)(-1824970232);
            public static SpawnFXHashes CarboliteBubbles = (SpawnFXHashes)(-2047541423);
            public static SpawnFXHashes MeteorImpactSteam = (SpawnFXHashes)1804074123;
        }

        public class SpaceDebris
        {

            public static float itemChance = 0.3f;

            public static List<WeightedElement> elements = new List<WeightedElement>()
            {
                new WeightedElement(SimHashes.Obsidian, 1),
                new WeightedElement(SimHashes.MaficRock, 1),
                new WeightedElement(SimHashes.Aluminum, 0.5f),
                new WeightedElement(SimHashes.Wolframite, 0.3f),
                new WeightedElement(SimHashes.Steel, 0.1f),
                new WeightedElement(SimHashes.SuperInsulator, 0.03f),
                new WeightedElement(SimHashes.Polypropylene, 0.8f)
            };

            public static List<WeightedName> items = new List<WeightedName>()
            {
                new WeightedName(GeneShufflerRechargeConfig.ID, 0.1f), // neural vacillator recharge
                new WeightedName(PowerStationToolsConfig.ID, 1f) // microchip
            };
        }


        public static HashSet<string> meteors = new HashSet<string>()
        {
            GoldCometConfig.ID,
            CopperCometConfig.ID,
            Meteors.IceCometConfig.ID,
            Meteors.TungstenCometConfig.ID,
            Meteors.AbyssaliteCometConfig.ID,
            Meteors.SpaceDebrisConfig.ID,
            Meteors.MegaMeteorConfig.ID
        };

        public struct WeightedElement : IWeighted
        {
            public SimHashes Element { get; }
            public float weight { get; set; }

            public WeightedElement(SimHashes element, float weight)
            {
                Element = element;
                this.weight = weight;
            }
        }


    }
}
