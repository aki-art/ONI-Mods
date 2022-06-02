using KSerialization.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using static PrintingPodRecharge.Cmps.ImmigrationModifier;

namespace PrintingPodRecharge
{
    public class BundleData
    {
        public Bundle Bundle { get; set; }

        public Mode PackageMode { get; set; }

        public string ColorHex { get; set; }

        public bool EnabledWithNoSpecialCarepackages { get; set; }

        public MinMax DuplicantCount { get; set; }

        public MinMax ItemCount { get; set; }

        [JsonIgnore]
        public Color? Color { get; set; }

        public List<PackageData> Packages { get; set; } = new List<PackageData>();

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

       // [JsonConverter(typeof(StringEnumConverter))]
        public enum Mode
        {
            Merge,
            Replace
        }
    }
}
