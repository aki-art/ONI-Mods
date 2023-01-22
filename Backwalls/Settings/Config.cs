using FUtility;
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

        public void Validate()
        {
            if(!ValidateColor(DefaultColor))
            {
                DefaultColor = new Color(0.47058824f, 0.47058824f, 0.47058824f).ToHexString();
            }

            if(!ValidatePattern(DefaultPattern))
            {
                DefaultPattern = "Tile";
            }
        }

        public bool ValidateColor(string color)
        {
            if (color.IsNullOrWhiteSpace())
            {
                return false;
            }

            if(!(color.Length == 6 || color.Length == 8))
            {
                return false;
            }

            if(!long.TryParse(DefaultColor, System.Globalization.NumberStyles.HexNumber, null, out _))
            {
                return false;
            }

            return true;
        }

        public bool ValidatePattern(string pattern)
        {
            if(Assets.Prefabs == null)
            {
                Log.Warning("Trying to check if a prefab exists before prefabs are loaded.");
                return false;
            }

            return Assets.TryGetPrefab(pattern) != null;
        }
    }
}
