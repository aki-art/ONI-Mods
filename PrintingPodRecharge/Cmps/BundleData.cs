using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace PrintingPodRecharge.Cmps
{
    public class BundleData
    {
        public ImmigrationModifier.Bundle ID { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BgColor { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FXColor { get; set; }

        public int PackageCount { get; set; }

        public MinMax DupeCount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Package[] Packages { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DuplicantConfig DuplicantOptions{ get; set; }

        [Serializable]
        public class DuplicantConfig
        {
            public Rarity TraitRarity { get; set; } = Rarity.Common;

            public bool AllowVacillatorTraits { get; set; } = false;

            public MinMax SkillBudget { get; set; } = MinMax.ZERO;
        }

        [Serializable]
        public class MinMax
        {
            public static MinMax ZERO = new MinMax(0, 0);
            public MinMax(int min, int max)
            {
                Min = min;
                Max = max;
            }

            public int Min { get; set; }

            public int Max { get; set; }
        }

        public class Package
        {
            public string PrefabID { get; set; }

            public float Amount { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public List<BundleFunction> Conditions { get; set; }

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public List<string> ConditionArguments { get; set; }
            
            public Package(string prefabID, float amount = 1f)
            {
                PrefabID = prefabID;
                Amount = amount;
            }

            public Package AddCondition(BundleFunction fn, string arg = null)
            {
                if (Conditions == null)
                {
                    Conditions = new List<BundleFunction>();
                }

                if (ConditionArguments == null)
                {
                    ConditionArguments = new List<string>();
                }

                if (fn == BundleFunction.Discovered && arg == null)
                {
                    arg = PrefabID;
                }

                Conditions.Add(fn);
                ConditionArguments.Add(arg);

                return this;
            }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum BundleFunction
        {
            None,
            CycleCount,
            Discovered
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum Rarity
        {
            Common,
            Uncommon,
            Epic
        }
    }
}
