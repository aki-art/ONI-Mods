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
        public string PipTreat { get; set; } = GrilledPrickleFruitConfig.ID;

        [JsonProperty]
        public bool SpawnGhostPip { get; set; } = true;

        [JsonProperty]
        public bool GhostPipEmitsLight { get; set; } = true;

        [JsonProperty]
        public bool PumpkinRequiresRot { get; set; } = true;

        public enum SpooksSetting
        {
            October,
            Always,
            Never
        }
    }
}
