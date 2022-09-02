using FUtility.SaveData;
using UnityEngine;

namespace ZipLine
{
    public class Config : IUserSetting
    {
        public int MaxLength { get; set; } = 64;

        public float SpeedModifier { get; set; } = 1.0f;

        public string RopePathVisualizerColor { get; set; } = new Color(1, 1, 1, 0.05f).ToHexString();

        public string RopePathVisualizerInvalidColor { get; set; } = new Color(1, 0, 0, 0.2f).ToHexString();
    }
}
