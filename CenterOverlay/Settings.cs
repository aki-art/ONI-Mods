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
            public int Offset { get; set; } = 0;
            [JsonProperty]
            public string VanillaColor { get; set; } = "41414FFF";
            public string ModdedColor { get; set; } = "41414FFF";
    }
}
