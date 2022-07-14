using FUtility.SaveData;
using Newtonsoft.Json;
using System;

namespace CompactMenus
{
    public class Config : IUserSetting
    {
        public BuildMenuSettings BuildMenu { get; set; } = new BuildMenuSettings();

        [Serializable]
        public class BuildMenuSettings
        {
            public bool SearchBar { get; set; } = true;

            public bool ShowTitles { get; set; } = true;

            public bool RememberLastSearch { get; set; } = true;

            public bool UseSubCategories { get; set; } = false;

            public float Scale { get; set; } = 0.46f;

            [JsonIgnore]
            public float CardWidth => 100f * Scale;

            [JsonIgnore]
            public float CardHeight => ShowTitles ? CardWidth * 1.43f : CardWidth;
        }
    }
}
