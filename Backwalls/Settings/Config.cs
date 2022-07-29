using FUtility.SaveData;
using Newtonsoft.Json;
using UnityEngine;

namespace Backwalls.Settings
{
    public class Config : IUserSetting
    {
        public WallLayer Layer { get; set; } = WallLayer.Automatic;

        [JsonIgnore]
        public Grid.SceneLayer SceneLayer { get; set; }

        public bool CopyPattern { get; set; } = true;

        public bool CopyColor { get; set; } = true;

        public string DefaultColor { get; set; } = new Color(0.47058824f, 0.47058824f, 0.47058824f).ToHexString();

        public string DefaultPattern { get; set; } = "Tile";

        public bool ShowHSVSliders { get; set; } = true;

        public bool ShowColorSwatches { get; set; } = true;

        public enum WallLayer
        {
            Automatic,
            BehindPipes,
            HidePipes
        }
    }
}
