using DecorPackA.DPBuilding.StainedGlassTile;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA
{
    public class ModAssets
    {
        public class Tags
        {
            public static Tag stainedGlassDye = TagManager.Create("StainedGlassMaterial", "Glass Dye");
            public static Tag colorShifty = TagManager.Create("ColorShifty");
            public static Tag stainedGlass = TagManager.Create("StainedGlass", "Stained Glass");
            public static Tag noPaintTag = TagManager.Create("NoPaint"); // MaterialColor mod uses this
        }

        public class Colors
        {
            public static Color gold = new Color(0.5f, 1f, 0.76f);
            public static Color uraniumGreen = new Color(0f, 3f, 0.6f);
            public static Color extraPink = new Color(2.5f, 0, 1.5f);
            public static Color palePink = new Color(1f, 0.6f, 0.7f);
            public static Color lavender = new Color(0.7f, 0.6f, 1f);
        }

        public static Dictionary<Tag, Tag> tiles = new Dictionary<Tag, Tag>()
        {
            { SimHashes.Aluminum.CreateTag(), AluminumSGTConfig.ID },
            { SimHashes.Cobalt.CreateTag(), CobaltSGTConfig.ID },
            { SimHashes.Copper.CreateTag(), CopperSGTConfig.ID },
            { SimHashes.DepletedUranium.CreateTag(), DepletedUraniumSGTConfig.ID },
            // { SimHashes.Diamond.CreateTag(), DiamondSGTConfig.ID },
            { SimHashes.Gold.CreateTag(), GoldSGTConfig.ID },
            { SimHashes.Granite.CreateTag(), GraniteSGTConfig.ID },
            { SimHashes.IgneousRock.CreateTag(), IgneousRockSGTConfig.ID },
            { SimHashes.Iron.CreateTag(), IronSGTConfig.ID },
            { SimHashes.Lead.CreateTag(), LeadSGTConfig.ID },
            { SimHashes.Rust.CreateTag(), RustSGTConfig.ID },
            { SimHashes.Salt.CreateTag(), SaltSGTConfig.ID },
            { SimHashes.SandStone.CreateTag(), SandStoneSGTConfig.ID },
            { SimHashes.SedimentaryRock.CreateTag(), SedimentaryRockSGTConfig.ID },
            { SimHashes.SlimeMold.CreateTag(), SlimeMoldSGTConfig.ID },
            { SimHashes.Steel.CreateTag(), SteelSGTConfig.ID },
            { SimHashes.Sucrose.CreateTag(), SucroseSGTConfig.ID },
            { SimHashes.SuperInsulator.CreateTag(), SuperInsulatorSGTConfig.ID },
            { SimHashes.Tungsten.CreateTag(), TungstenSGTConfig.ID },
        };
    }
}
