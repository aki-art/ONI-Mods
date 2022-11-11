using FUtility;
using FUtility.FUI;
using System.IO;
using UnityEngine;

namespace SnowSculptures
{
    public class ModAssets
    {
        public class Prefabs
        {
            public static GameObject snowParticlesPrefab;
            public static GameObject snowmachineSidescreenPrefab;
        }

        public static Tag customHighlightTag = TagManager.Create("Aki_CustomHighlightTag");

        public static class Sounds
        {
            public const string GLASS_SHATTER = "SnowSculptures_ShatterGlass";
            public const string CUICA_DRUM = "SnowSculptures_CuicaDrum";
        }

        public static void LoadAssets()
        {
            var bundle = FUtility.Assets.LoadAssetBundle("snowsculptures_assets");

            var emitterGo = bundle.LoadAsset<GameObject>("Assets/prefabs/SnowEmitter.prefab");
            Prefabs.snowParticlesPrefab = emitterGo.transform.Find("Particle System").gameObject;
            Prefabs.snowParticlesPrefab.SetLayerRecursively(Game.PickupableLayer);
            Prefabs.snowParticlesPrefab.SetActive(false);

            var material = new Material(Shader.Find("UI/Default"))
            {
                renderQueue = RenderQueues.Liquid, // Sparkle Streaker particles also render here
                mainTexture = bundle.LoadAsset<Texture2D>("Assets/Images/snow_particles 1.png")
            };

            Prefabs.snowParticlesPrefab.GetComponent<ParticleSystemRenderer>().material = material;

            Prefabs.snowmachineSidescreenPrefab = bundle.LoadAsset<GameObject>("Assets/UIs/SnowmachineSidescreen.prefab");

            var tmpConverter = new TMPConverter();
            tmpConverter.ReplaceAllText(Prefabs.snowmachineSidescreenPrefab);

            LoadSounds();
        }

        private static void LoadSounds()
        {
            var path = Path.Combine(Utils.ModPath, "assets");

            AudioUtil.LoadSound(Sounds.GLASS_SHATTER, Path.Combine(path, "452667__kyles__window-break-with-axe-glass-shatter-in-trailer.wav"));
            AudioUtil.LoadSound(Sounds.CUICA_DRUM, Path.Combine(path, "mus_drumcuica2.wav"));
        }
    }
}
