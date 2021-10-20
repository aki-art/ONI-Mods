using Newtonsoft.Json;
using System;
using TMPro;

namespace PollingOfONIUITest
{
    [Serializable]
    public class TMPData
    {
        [JsonProperty]
        public string Font { get; set; } = "NotoSans-Regular";
        [JsonProperty]
        public FontStyles FontStyle { get; set; }
        [JsonProperty]
        public float FontSize { get; set; }
        [JsonProperty]
        public string Alignment { get; set; }
        [JsonProperty]
        public int MaxVisibleLines { get; set; }
        [JsonProperty]
        public bool EnableWordWrapping { get; set; }
        [JsonProperty]
        public bool AutoSizeTextContainer { get; set; }
        [JsonProperty]
        public string Content { get; set; }
        [JsonProperty]
        public float X { get; set; }
        [JsonProperty]
        public float Y { get; set; }
    }
}
