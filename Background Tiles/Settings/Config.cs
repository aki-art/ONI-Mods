using FUtility.SaveData;

namespace BackgroundTiles.Settings
{
    public class Config : IUserSetting
    {
        public float MassModifier { get; set; } = .25f;

        public float DecorModifier { get; set; } = .25f;

        public float HitPointModifier { get; set; } = .5f;

        public bool CapDecorAt0 { get; set; } = true;

        public bool UseExtraBuildCategory { get; set; } = true;
    }
}
