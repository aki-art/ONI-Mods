using FUtility.SaveData;
using Newtonsoft.Json;
using System;

namespace PrintingPodRecharge.Settings
{
    public class General : IUserSetting
    {
        public float RandomDupeReplaceChance { get; set; } = 0.5f;

        public float RefundBioInkKg { get; set; } = 1f;

        public bool RefundActiveInk { get; set; } = true;

        [JsonIgnore]
        public int EggCycle { get; set; } = 225;

        [JsonIgnore]
        public int RainbowEggCycle { get; internal set; } = 250;
    }
}
