using System.IO;
using System.Reflection;
using UnityEngine;
using WorldTraitsPlus.Settings;

namespace WorldTraitsPlus
{
    class ModAssets
    {
        private static AssetBundle assetBundle;
        public static GameObject traitsSelectorPrefab;
        public static GameObject sinkholeFX;
        public static AudioClip earthquakeRumbleSound;
        public static AudioClip test;
        public static Texture2D QuakeVignette;
        public static UserSettings settings;
        public static string ModPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        internal static void Initialize()
        {
            settings = new UserSettings();
        }

        internal static void LateLoadAssets()
        {
            assetBundle = FUtility.Assets.LoadAssetBundle("wtp_uiprefab");
            traitsSelectorPrefab = assetBundle.LoadAsset<GameObject>("TraitContainer");
            sinkholeFX = assetBundle.LoadAsset<GameObject>("sinkholeFX");
            earthquakeRumbleSound = assetBundle.LoadAsset<GameObject>("EarthQuake").GetComponent<AudioSource>().clip;
            test = assetBundle.LoadAsset("stonec") as AudioClip;
            Debug.Log(test.length);

            //var test = FUtility.Assets.LoadUIPrefab("wtp_uiprefab", "EarthQuake").GetComponent<AudioSource>().clip;
            FUtility.FUI.TMPConverter.ReplaceAllText(traitsSelectorPrefab);
            QuakeVignette = FUtility.Assets.LoadTexture("vignette-quake");

        }

    }
}
