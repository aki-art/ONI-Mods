using FUtility;
using PrintingPodRecharge.Content.Cmps;
using System;
using System.Collections.Generic;

namespace PrintingPodRecharge
{
    public class BundleData
    {
        public int Version { get; set; } = 0;

        public Bundle Bundle { get; set; }

        public string ColorHex { get; set; }

        public bool OverrideInternalLogic { get; set; } = false;

        public string Background { get; set; } = "rpp_greyscale_dupeselect_kanim";

        public bool EnabledWithNoSpecialCarepackages { get; set; }

        public MinMax DuplicantCount { get; set; }

        public MinMax ItemCount { get; set; }

        public Dictionary<string, float> Data { get; set; } = new Dictionary<string, float>();

        public List<PackageData> Packages { get; set; } = new List<PackageData>();

        public List<string> BlackList { get; set; } = new List<string>();

        public float GetOrDefault(string key, float defaultValue)
        {
            if (Data.TryGetValue(key, out var result))
            {
                return result;
            }

            return defaultValue;
        }

        public int GetOrDefault(string key, int defaultValue)
        {
            return (int)GetOrDefault(key, (float)defaultValue);
        }

        [Serializable]
        public class MinMax
        {
            public static MinMax None = new MinMax(0, 0);

            public MinMax(int min, int max)
            {
                Min = min;
                Max = max;
            }

            public int Min { get; set; }

            public int Max { get; set; }
        }
    }
}
