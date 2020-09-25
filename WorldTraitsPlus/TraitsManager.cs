using FUtility;
using Klei.CustomSettings;
using System.Linq;
using UnityEngine;
using WorldTraitsPlus.Traits.WorldEvents;

namespace WorldTraitsPlus.Traits
{
    class TraitsManager : KMonoBehaviour
    {
        public static TraitsManager Instance { get; private set; }
        public SeededRandom random;

        protected override void OnPrefabInit()
        {
            Instance = this;
        }

        public void Initialize()
        {
            var worldSeed = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.WorldgenSeed);
            int seed = int.Parse(worldSeed.id);
            random = new SeededRandom(seed);

            UpdateTraits();
            gameObject.SetActive(true);
        }

        public void UpdateTraits()
        {
            if (SaveLoader.Instance.GameInfo.worldTraits != null)
            {
                if (SaveLoader.Instance.GameInfo.worldTraits.Contains("traits/ice_comets"))
                {
                    //gameObject.AddComponent<Traits.IceComet>();
                }
                if (SaveLoader.Instance.GameInfo.worldTraits.Contains("traits/earthquakes"))
                {
                    //gameObject.AddComponent<Traits.IceComet>();
                }

                //WorldEventManager.Instance.SpawnEvent();
            }
        }


        public static void DestroyInstance() => Instance = null;
    }
}
