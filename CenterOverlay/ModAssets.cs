using UnityEngine;

namespace CenterOverlay
{
    class ModAssets
    {
        public static GameObject settingsScreenPrefab;
        public static Texture2D symmetryOverlayTexture;
        public static Settings Settings { get; set; }
        public static void Initialize()
        {
            Settings = new Settings();
            symmetryOverlayTexture = FUtility.Assets.LoadTexture("overlay_symmetry");

        }
    }
}
