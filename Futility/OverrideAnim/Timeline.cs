using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FUtility.OverrideAnim
{
    public class Timeline<T> where T : AnimKey
    {
        private T currentKey;
        private T nextKey;

        private int index = 0;

        private readonly List<T> keySet;

        private float elapsedSinceLastKey;

        public void Next(KBatchedAnimController kbac)
        {
            if (nextKey is null) return;

            currentKey.Stop(kbac);
            nextKey.Start(kbac);

            currentKey = nextKey;
            nextKey = keySet.Count - 1 >= index ? null : keySet.ElementAt(index++);

            elapsedSinceLastKey = 0;
        }

        public void EndAll(KBatchedAnimController kbac)
        {
            keySet.ForEach(k => k.Stop(kbac));
        }

        public bool IsEmpty() => keySet.Count == 0;

        public void Sort(float animationLength)
        {
            if (IsEmpty()) return;

            keySet.OrderBy(k => k.time);

            for (int i = 0; i < keySet.Count; i++)
            {
                T key = keySet[i];
                float nextTime = (i == keySet.Count - 1) ? animationLength : keySet[i + 1].time;
                key.length = nextTime - key.time;
            }

            currentKey = keySet.ElementAt(index++);
            nextKey = keySet.Count > 1 ? keySet.ElementAt(index++) : null;

            Log.Debuglog($"Created and sorted new timeline \n {this}");
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"Timeline {typeof(T)}: \n");

            keySet.ForEach(k => stringBuilder
                .Append(" -  ")
                .Append(k.ToString())
                .Append("\n"));

            return stringBuilder.ToString();
        }

        public void Update(KBatchedAnimController kbac, float dt)
        {
            if (currentKey is null)
            {
                return;
            }

            elapsedSinceLastKey += dt;

            if (elapsedSinceLastKey >= currentKey.length)
            {
                Next(kbac);
            }

            currentKey.Update(kbac, dt, elapsedSinceLastKey);
        }

        public void Add(T key)
        {
            keySet.Add(key);
        }
    }
}
