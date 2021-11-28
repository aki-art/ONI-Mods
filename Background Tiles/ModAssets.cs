using UnityEngine;

namespace BackgroundTiles
{
    public class ModAssets
    {
        public static Texture2D uiShadow;

        public static void LoadAssets()
        {
            uiShadow = FUtility.Assets.LoadTexture("ui_shadow");
        }

        public class Tags
        {
            // use to prevent a tile from generating a backwall version
            public static readonly Tag noBackwall = TagManager.Create("NoBackwall");

            // used for already generated tiles, for replacement tags
            public static readonly Tag backWall = TagManager.Create("backWall");

            public static readonly Tag stainedGlass = TagManager.Create("DecorPackA_StainedGlass");
            public static readonly Tag stainedGlassDye = TagManager.Create("DecorPackA_StainedGlassMaterial");
            public static readonly Tag extr = TagManager.Create("DecorPackA_StainedGlassDye");
        }
    }
}
