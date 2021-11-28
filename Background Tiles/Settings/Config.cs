using FUtility.SaveData;
using Newtonsoft.Json;

namespace BackgroundTiles.Settings
{
    public class Config : IUserSetting
    {
        public float MassModifier { get; set; } = .25f;

        public float DecorModifier { get; set; } = .25f;

        public float HitPointModifier { get; set; } = .5f;

        public bool CapDecorAt0 { get; set; } = true;

        public bool UseExtraBuildCategory { get; set; } = true;

        public string OverlayColorHex { get; set; } = "FF0000";

        [JsonIgnore]
        public bool UseLogicGatesFrontSceneLayer { get; set; } = false;
    }
}
