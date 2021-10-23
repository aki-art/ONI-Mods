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
           // public static Tag colorShifty = TagManager.Create("ColorShifty");
            public static Tag stainedGlass = TagManager.Create("StainedGlass", "Stained Glass");
            public static Tag noPaintTag = TagManager.Create("NoPaint"); // MaterialColor mod needs this
        }

        public class Colors
        {
            public static Color gold = new Color(1f, 0.86f, 0.33f);
        }

        public static Dictionary<Tag, Tag> tiles = new Dictionary<Tag, Tag>()
        {
            { SimHashes.Cobalt.CreateTag(), CobaltSGTConfig.ID },
            { SimHashes.Copper.CreateTag(), CopperSGTConfig.ID },
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
            { SimHashes.SuperInsulator.CreateTag(), SuperInsulatorSGTConfig.ID },
        };
    }
}
