using ProcGen;
using System.Collections.Generic;
using System.Linq;

namespace FUtility
{
	public static class Extensions
	{
		public static T GetWeightedRandom<T>(this IEnumerable<T> enumerator, SeededRandom rand = null) where T : IWeighted
		{
			if (enumerator == null || enumerator.Count() == 0) return default;

			float totalWeight = enumerator.Sum(n => n.weight);
			float treshold = rand == null ? UnityEngine.Random.value : rand.RandomValue();
			treshold *= totalWeight;

			float num3 = 0.0f;
			foreach (T item in enumerator)
			{
				num3 += item.weight;
				if (num3 > treshold)
				{
					return item;
				}
			}

			return enumerator.GetEnumerator().Current;
		}
	}
}
