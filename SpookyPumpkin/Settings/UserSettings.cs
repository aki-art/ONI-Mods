using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace SpookyPumpkin.Settings
{
    [Serializable]
    public class UserSettings
    {
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public SpooksSetting Spooks { get; set; } = SpooksSetting.October;

        [JsonProperty]
        public bool SpawnGhostPip { get; set; } = true;

        [JsonProperty]
        public bool GhostPipEmitsLight { get; set; } = true;

        [JsonProperty]
        public bool PumpkinRequiresRot { get; set; } = true;

/*        [JsonProperty]
        public float PumpkinPlantLifeCycle { get; set; } = 8f;

        [JsonProperty]
        public int PumpkinPlantProduce { get; set; } = 2;

        [JsonProperty]
        public int RawPumpkinCalories { get; set; } = 600;

        [JsonProperty]
        public int RawPumpkinQuality { get; set; } = -2;

        [JsonProperty]
        public int PieCalories { get; set; } = 4000;

        [JsonProperty]
        public int PieQuality { get; set; } = 4;

        [JsonProperty]
        public int ToastedSeedCalories { get; set; } = 400;

        [JsonProperty]
        public int ToastedSeedQuality { get; set; } = 2;*/

        public enum SpooksSetting
        {
            October,
            Always,
            Never
        }
    }
}
