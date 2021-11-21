using UnityEngine;

namespace DecorPackA
{
    public class ModAssets
    {
        public class Prefabs
        {
            public static GameObject aquariumSideScreen;
        }

        public class Tags
        {
            public static Tag stainedGlassDye = TagManager.Create(Mod.PREFIX + "StainedGlassMaterial");
            public static Tag stainedGlass = TagManager.Create(Mod.PREFIX + "StainedGlass", "Stained Glass");
            public static Tag noPaintTag = TagManager.Create("NoPaint"); // MaterialColor mod uses this

            public static TagSet extraGlassDyes = new TagSet();
        }

        public class Colors
        {
            // "out of range" values are used to make some effect stronger, Unity does not clamp color values
            public static Color bloodRed = Color.red * 1.7f;
            public static Color gold = new Color(1.3f, 0.96f, 0.5f);
            public static Color uraniumGreen = new Color(0f, 3f, 0.6f);
            public static Color extraPink = new Color(1.5f, 0, 0.7f);
            public static Color palePink = new Color(1f, 0.6f, 0.7f);
            public static Color lavender = new Color(0.7f, 0.6f, 1f);
            public static Color W_H_I_T_E = new Color(15f, 15f, 15f);
        }
    }
}
