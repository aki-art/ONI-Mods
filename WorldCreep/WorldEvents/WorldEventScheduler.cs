using FUtility;
using UnityEngine;
using WorldCreep.Buildings;

namespace WorldCreep.WorldEvents
{
    public class WorldEventScheduler : KMonoBehaviour
    {
        public static WorldEventScheduler Instance { get; private set; }
        public Components.Cmps<WorldEvent> WorldEvents = new Components.Cmps<WorldEvent>();
        public Components.Cmps<SeismoGraph> SeismoGraphs = new Components.Cmps<SeismoGraph>();
        public WorldEvent currentEvent;

        protected override void OnPrefabInit()
        {
            Instance = this;
            WorldEvents.Register(OnEventSpawned, OnEventRemoved);
            Subscribe((int)WorldEventHashes.EventEnded, OnEventEnded);
        }
        protected override void OnSpawn()
        {
            base.OnSpawn();
            UpdateCurrentEvent();
        }

        private void UpdateCurrentEvent()
        {
            if (WorldEvents != null && WorldEvents.Count > 0)
            {
                WorldEvents.Items.Sort((w1, w2) => w1.StartingIn.CompareTo(w2.StartingIn));
                currentEvent = WorldEvents[0];
            }
            else
            {
                Debug.Log("No more events");
                currentEvent = null;
            }
        }

        private void OnEventEnded(object obj)
        {
            //Schedule(new WorldEvent());
            UpdateCurrentEvent();
        }

        public void StartEvent(WorldEvent we)
        {
            Trigger((int)WorldEventHashes.EventStarted, we);
            we.Begin();
        }

        public void StopAll()
        {
            foreach(WorldEvent worldevent in WorldEvents)
            {
                worldevent.End();
            }
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
            {
                // TODO: Seeded Random
                float time = Random.Range(ModSettings.WorldEvents.MinDelayBetweenEvents, ModSettings.WorldEvents.MaxDelayBetweenEvents);
                Schedule(we, time);
                UpdateCurrentEvent();
            }
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
                we.randomize = true;
            }

            return we;
        }

        public static void DestroyInstance() => Instance = null;

#if DEBUG
        // Displays a small UI showing Debug information
        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(200, 200, 200, 600));
            GUILayout.BeginVertical();
            GUILayout.Box("World Events");

            foreach (WorldEvent ev in WorldEvents)
            {
                GUILayout.Box(ev.GetProperName());
                GUILayout.Label("Power: " + ev.power);
                if (ev.Stage == WorldEvent.WorldEventStage.Ongoing)
                {
                    GUILayout.Label("<color=red><b>Active</b></color> ");
                }
                else
                    GUILayout.Label($"<color=yellow><b>Starting in: {ev.schedule.TimeRemaining}</b></color> ");
                if (GUILayout.Button("Find"))
                    CameraController.Instance.CameraGoTo(ev.transform.position);
                GUILayout.Space(20);
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
#endif
    }
}
