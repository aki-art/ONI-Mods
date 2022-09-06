using FUtility.SaveData;
using UnityEngine;

namespace ZipLine
{
    public class Config : IUserSetting
    {
        public int MaxLength { get; set; } = 64;

        public float FiberPerMeterCost { get; set; } = 0.2f;

        public float MinimumAngleAllowed { get; set; } = 30f;

        public float SpeedModifier { get; set; } = 1.0f;

        public float KJPerMeter { get; set; } = 0.1f;

        public float AccelerationMultiplier { get; set; } = 1.05f;

        public int FreeHorizontalTiles { get; set; } = 5;

        public string RopePathVisualizerColor { get; set; } = new Color(1, 1, 1, 0.05f).ToHexString();

        public string RopePathVisualizerInvalidColor { get; set; } = new Color(1, 0, 0, 0.2f).ToHexString();
    }
}
