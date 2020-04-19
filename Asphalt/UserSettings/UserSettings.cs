using Newtonsoft.Json;
using System;

namespace Asphalt
{
    [Serializable]
    public class UserSettings
    {
        [JsonProperty]
        public float SpeedMultiplier { get; set; } = 0.33333f;
        [JsonProperty]
        public bool UseSafeFolder { get; set; } = true;
        [JsonProperty]
        public string BitumenColor { get; set; } = "41414FFF";
    }
}
