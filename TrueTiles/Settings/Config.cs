﻿using FUtility.SaveData;
using Newtonsoft.Json;

namespace TrueTiles.Settings
{
    public class Config : IUserSetting
    {
        public float Speed { get; set; } = 2.0f;

        [JsonIgnore]
        public bool SpeedChanged { get; set; } = false;
    }
}
