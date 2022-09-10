using FUtility.SaveData;
using Newtonsoft.Json;

namespace PrintingPodRecharge.Settings
{
    public class General : IUserSetting
    {
        public float RandomDupeReplaceChance { get; set; } = 0.5f;

        public bool UseCustomPacks { get; set; } = true;

        [JsonIgnore]
        public int EggCycle { get; set; } = 225;

        [JsonIgnore]
        public int RainbowEggCycle { get; internal set; } = 250;
    }
}
