using FUtility.SaveData;

namespace Backwalls.Settings
{
    public class Config : IUserSetting
    {
        public bool HideUtilities { get; set; } = false;

        public bool CopyPattern { get; set; } = true;

        public bool CopyColor { get; set; } = true;

        public int DefaultColorIdx { get; set; } = 41;

        public string DefaultPattern { get; set; } = "Tile";
    }
}
