using ProcGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WorldTraitsPlus
{
    public static class ExtensionMethods
    {
        public static T1 GetRandomKey<T1, T2>(this Dictionary<T1, T2> dict)
        {
            if (dict.Count == 0) return default;
            List<T1> keys = Enumerable.ToList(dict.Keys);
            return keys.GetRandom();
        }

        public static T1 ChooseWeighted<T1, T2>(this Dictionary<T1, T2> dict, SeededRandom rand) where T2 : IWeighted
        {
            if (dict.Count == 0)
                return default;
            float totalWeight = 0.0f;
            foreach (var kvp in dict)
                totalWeight += kvp.Value.weight;

            float treshold = rand.RandomValue() * totalWeight;
            float num3 = 0.0f;
            foreach (var kvp in dict)
            {
                num3 += kvp.Value.weight;
                if (num3 > treshold)
                    return kvp.Key;
            }

            return dict.Keys.GetEnumerator().Current;
        }
      
    }
}

