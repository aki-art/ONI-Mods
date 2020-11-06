/*using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WorldCreep.WorldEvents
{
    public class ScheduledList<T> : ICollection, IEnumerable
    {
        public List<ScheduledEvent> items;

        public int Count => items.Count;

        public void Add(T ev, float time, Action<object> callback, object data = null)
        {
            callback += o => Remove(ev);
            items.Add(new ScheduledEvent()
            {
                ev = ev,
                handle = GameScheduler.Instance.Schedule("Begin" + ev.GetType().Name, time, callback, data)
            });

            items.Sort();
        }

        public bool Contains (T ev)
        {
            return items.Any(i => i.ev.Equals(ev));
        }

        public void Remove(T item)
        {
            ScheduledEvent ev = items.FirstOrDefault(i => item.Equals(i.ev));
            if(ev.handle.IsValid)
                ev.handle.ClearScheduler();
            items.Remove(ev);
        }

        public T Last => items == null || items.Count < 1 ? default : items[Count - 1].ev;

        public T PeekNext => items == null || items.Count < 1 ? default : items[0].ev;

        public T PopNext()
        {
            if(items == null || items.Count < 1)
            {
                Debug.LogWarning("Cannot pop from an empty ScheduedList");
                return default;
            }

            T next =  items[0].ev;
            Remove(next);
            return next;
        }

        public float LastSchedule => items[Count - 1].handle.TimeRemaining;

        public object SyncRoot => throw new NotImplementedException();

        public bool IsSynchronized => throw new NotImplementedException();

        public IEnumerator GetEnumerator() => items.GetEnumerator();

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public class ScheduledEvent : IComparable
        {
            public SchedulerHandle handle;
            public T ev;

            public float TimeRemaining => handle.TimeRemaining;

            public int CompareTo(object obj)
            {
                if (obj == null) return 1;

                if (obj is ScheduledEvent other)
                    return TimeRemaining.CompareTo(other.TimeRemaining);
                else
                    throw new ArgumentException("Object is not a ScheduledEvent");
            }
        }
    }
}
*/