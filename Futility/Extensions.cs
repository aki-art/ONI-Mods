using ProcGen;
using System.Collections.Generic;
using System.Linq;

namespace FUtility
{
    public static class Extensions
    {
        public static T AddOrGet<K, T>(this IDictionary<K, T> dictionary, K key, T value)
        {
            if(!dictionary.ContainsKey(key)) 
            {
                dictionary.Add(key, value);
            }

            return dictionary[key];
        }

        public static void AddAll<T>(this IEnumerable<T> enumerator, params T[] items)
        {
            AddAll(enumerator, items.AsEnumerable());
        }

        public static void AddAll<T>(this IEnumerable<T> enumerator, IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                enumerator.Append(item);
            }
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerator)
        {
            return enumerator != null && enumerator.Count() > 0;
        }

        public static T GetWeightedRandom<T>(this IEnumerable<T> enumerator, SeededRandom rand = null) where T : IWeighted
        {
            if (enumerator.IsNullOrEmpty()) return default;
            float totalWeight = enumerator.Sum(n => n.weight);
            float treshold = rand == null ? UnityEngine.Random.value : rand.RandomValue();
            treshold *= totalWeight;

            float num3 = 0.0f;
            foreach (T item in enumerator)
            {
                num3 += item.weight;
                if (num3 > treshold)
                    return item;
            }

            return enumerator.GetEnumerator().Current;
        }

        public static T GetRandomAny<T>(this IEnumerable<T> enumerator, SeededRandom rand = null)
        {
            if (enumerator.IsNullOrEmpty()) return default;
            int index = rand == null ? UnityEngine.Random.Range(0, enumerator.Count()) : rand.RandomRange(0, enumerator.Count());

            return enumerator.ElementAt(index);
        }
    }
}
