using Newtonsoft.Json;
using System;

namespace WorldCreep
{
    [Serializable]
    public class WorldEventSettings
    {
        public WorldEventSettings()
        {
            DisableWorldEvents = false;
            MinDelayBetweenEvents = 10f;
            MaxDelayBetweenEvents = 40f;
            SafeZoneRadius = 25;
            ProtectedPrefabs = new string[] 
            {
                HeadquartersConfig.ID,
                MassiveHeatSinkConfig.ID
            };

            EarthQuake = new WorldEventInfo()
            {
                Weight = 1f,
                MinDuration = 40f,
                MaxDuration = 100f,
                MinPower = 0.2f,
                MaxPower = 0.9f,
                Disabled = false,
                CanDestroyBlocks = true,
                CanSpawnGeyser = true
            };

            SinkHole = new WorldEventInfo()
            {
                Weight = 0.5f,
                MinDuration = 20f,
                MaxDuration = 50f,
                MinPower = 1f,
                MaxPower = 10f,
                Disabled = false,
                CanDestroyBlocks = true,
                CanDestroyBackwall = true
            };

            MagneticStorm = new WorldEventInfo()
            {
                Weight = 0.5f,
                MinDuration = 300f,
                MaxDuration = 1200f,
                MinPower = 1f,
                MaxPower = 5f,
                Disabled = false
            };

            Drought = new WorldEventInfo()
            {
                Weight = 0.5f,
                MinDuration = 300f,
                MaxDuration = 1200f,
                MinPower = 1f,
                MaxPower = 5f,
                Disabled = false
            };

            CoreOverPressure = new WorldEventInfo()
            {
                Weight = 0.5f,
                MinDuration = 300f,
                MaxDuration = 1200f,
                MinPower = 1f,
                MaxPower = 5f,
                Disabled = false,
                HeatMultiplier = 2f,
                OutputMultiplier = 1.5f
            };
        }

        public bool DisableWorldEvents { get; set; }
        public float MinDelayBetweenEvents { get; set; }
        public float MaxDelayBetweenEvents { get; set; }
        public int SafeZoneRadius { get; set; }

        public string[] ProtectedPrefabs { get; set; }

        public WorldEventInfo EarthQuake { get; set; }
        public WorldEventInfo SinkHole { get; set; }
        public WorldEventInfo CoreOverPressure { get; set; }
        public WorldEventInfo MagneticStorm { get; set; }
        public WorldEventInfo Drought { get; set; }
    }


    [Serializable]
    public class WorldEventInfo
    {
        public float Weight { get; set; }
        public float MinDuration { get; set; }
        public float MaxDuration { get; set; }
        public float MinPower { get; set; }
        public float MaxPower { get; set; }
        public bool Disabled { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float? OutputMultiplier { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public float? HeatMultiplier { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? CanDestroyBlocks { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? CanDestroyBackwall { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? CanSpawnGeyser { get; set; }
    }
}
