using UnityEngine;

namespace DecorPackA
{
    public class ModAssets
    {
        public class Tags
        {
            public static Tag stainedGlassDye = TagManager.Create(Mod.PREFIX + "StainedGlassMaterial");
            public static Tag stainedGlass = TagManager.Create(Mod.PREFIX + "StainedGlass", "Stained Glass");
            public static Tag noPaint = TagManager.Create("NoPaint"); // MaterialColor mod uses this
            public static Tag noBackwall = TagManager.Create("NoBackwall"); // Background Tiles mod uses this

            public static TagSet extraGlassDyes = new();
        }

        public class Colors
        {
            // "out of range" values are used to make some effect stronger, Unity does not clamp color values
            public static Color bloodRed = Color.red * 1.7f;
            public static Color gold = new(1.3f, 0.96f, 0.5f);
            public static Color uraniumGreen = new(0f, 3f, 0.6f);
            public static Color extraPink = new(1.5f, 0, 0.7f);
            public static Color extraGreen = new(0f, 16.45f, 12.11f);
            public static Color extraOrange = new(2.55f, 0.66f, 0);
            public static Color palePink = new(1f, 0.6f, 0.7f);
            public static Color lavender = new(0.7f, 0.6f, 1f);
            public static Color corium = new(0f, 2f, 0);
            public static Color W_H_I_T_E = new(15f, 15f, 15f);
            public static Color abyssalite = new(0, 0.48f, 2.55f);
        }
    }
}
