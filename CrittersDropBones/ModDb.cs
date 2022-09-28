using System;
using System.Collections.Generic;

namespace CrittersDropBones
{
    public class ModDb
    {
        public class FoodEffects
        {
            public static Dictionary<string, Action<Worker>> foodEffects = new Dictionary<string, Action<Worker>>();

            public static void Add(string id, Action<Worker> fn)
            {
                foodEffects.Add(id, fn);
            }

            public static void Apply(string id, Worker worker)
            {
                if (foodEffects.TryGetValue(id, out var fn))
                {
                    fn(worker);
                }
            }
        }
    }
}
