using FUtility.SaveData;

namespace TrueTiles.Settings
{
    public class Config : IUserSetting
    {
        public bool SaveExternally { get; set; } = true;
    }
}
