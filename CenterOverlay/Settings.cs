using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CenterOverlay
{
    [Serializable]
    public class Settings
    {
            [JsonProperty]
            public  Offset { get; set; } = 0;
            [JsonProperty]
            public bool ModdedLeftSide { get; set; } = false;
            [JsonProperty]
            public string VanillaColorHex { get; set; } = "41414FFF";
            [JsonProperty]
            public string ModdedColorHex { get; set; } = "41414FFF";
    }
}
