using FUtility.FUI;
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
        }
    }
}
