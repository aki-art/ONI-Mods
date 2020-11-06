using FUtility;
using UnityEngine;

namespace WorldCreep.WorldEvents
{
    public class WorldEventScheduler : KMonoBehaviour
    {
        public static WorldEventScheduler Instance { get; private set; }
        public Components.Cmps<WorldEvent> WorldEvents = new Components.Cmps<WorldEvent>();

        protected override void OnPrefabInit()
        {
            Instance = this;
            WorldEvents.Register(OnEventSpawned, OnEventRemoved);
            Subscribe((int)WorldEventHashes.EventEnded, OnEventEnded);
        }

        private void OnEventEnded(object obj)
        {
            //Schedule(new WorldEvent());
        }

        public void StartEvent(WorldEvent we)
        {
            Trigger((int)WorldEventHashes.EventStarted, we);
            we.Begin();
        }

        private void OnEventRemoved(WorldEvent we)
        {
            Trigger((int)WorldEventHashes.EventEnded, we);
        }

        private void OnEventSpawned(WorldEvent we)
        {
            Trigger((int)WorldEventHashes.EventScheduled, we);

            if (we.immediateStart)
                StartEvent(we);
            else
                // TODO: Seeded Random
                Schedule(we, Random.Range(ModSettings.WorldEvents.MinDelayBetweenEvents, ModSettings.WorldEvents.MaxDelayBetweenEvents));
        }

        public void Schedule(WorldEvent we, float time)
        {
            we.schedule = GameScheduler.Instance.Schedule("BeginWorldEvent", time, o => StartEvent(o as WorldEvent), we);
        }

        public WorldEvent CreateEvent(string PrefabID, Vector3 location, float power = -1, float duration = -1, bool immediateStart = false)
        {
            WorldEvent we = Utils.Spawn(PrefabID, location)?.GetComponent<WorldEvent>();
            if(we != null)
            {
                if (power > -1)
                    we.power = power;
                we.immediateStart = immediateStart;
            }

            return we;
        }

        public static void DestroyInstance() => Instance = null;
    }
}
