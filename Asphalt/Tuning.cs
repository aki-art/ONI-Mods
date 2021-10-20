using System.Collections.Generic;
using UnityEngine;
using static Asphalt.Settings.FPlaidSlider;
using static Asphalt.STRINGS.UI.ASPHALTSETTINGSDIALOG.CONTENT;

namespace Asphalt
{
    public class Tuning
    {
        public static Color32 bitumenElementColor = new Color32(65, 65, 79, 255);
        public const float BITUMEN_DROP = 100F;
        public const float DEFAULT_SPEED_MULTIPLIER = 2f;
        public static List<Range> SpeedRanges = new List<Range>
        {
            new Range(1f, SLIDERPANEL.TIER1_NOBONUS, Color.grey),
            new Range(1.05f, SLIDERPANEL.TIER2_SMALLBONUS, Color.grey),
            new Range(1.25f, SLIDERPANEL.TIER3_REGULARTILE, Color.white),
            new Range(1.3f,SLIDERPANEL.TIER4_SOMEBONUS, Color.white),
            new Range(1.5f,SLIDERPANEL.TIER5_METALTILE, Color.white),
            new Range(1.55f, SLIDERPANEL.TIER6_FAST, Color.white),
            new Range(2f,SLIDERPANEL.TIER7_DEFAULT, Color.white),
            new Range(2.05f, SLIDERPANEL.TIER8_GOFAST, new Color32(55, 168, 255, 255)),
            new Range(3f, SLIDERPANEL.TIER9_LIGHTSPEED, new Color32(183, 226, 13, 255)),
            new Range(5f, SLIDERPANEL.TIER10_RIDICULOUS, new Color32(226, 93, 13, 255)),
            new Range(20f, SLIDERPANEL.TIER11_LUDICROUS, new Color32(226, 23, 13, 255))
        };

        public static float ConvertSpeed(float input)
        {
            const float maxValue = 20f;
            const float e = 4.565703f;

            float result = input <= 1f / 3f ? 3f * input + 1 : maxValue * Mathf.Pow(input, e);

            result = Mathf.Clamp(result, 1f, maxValue);
            result = RoundTo05(result);

            return result;
        }

        public static float RoundTo05(float input) => Mathf.Round(input * 20) / 20;
    }
}
