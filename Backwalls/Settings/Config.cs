using FUtility.SaveData;
using UnityEngine;

namespace Backwalls.Settings
{
    public class Config : IUserSetting
    {
        public WallLayer Layer { get; set; } = WallLayer.Automatic;

        public string DefaultPattern { get; set; } = "Tile";

        public string DefaultColor { get; set; } = new Color(0.47058824f, 0.47058824f, 0.47058824f).ToHexString();

        public enum WallLayer
        {
            Automatic,
            BehindPipes,
            HidePipes
        }
    }
}
