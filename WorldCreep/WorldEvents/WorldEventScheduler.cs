using FUtility;
using ProcGen;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WorldCreep.WorldEvents
{
    public class WorldEventScheduler : KMonoBehaviour
    {
        public static WorldEventScheduler Instance { get; private set; }
        public Components.Cmps<WorldEvent> WorldEvents = new Components.Cmps<WorldEvent>();
        public WorldEvent currentEvent;
        public Activity ActivityLevel { get; private set; }
        private List<WorldEventInfo> worldEventInfos;
        private MinMax delay;
        SeededRandom random;

        protected override void OnPrefabInit()
        {
            Instance = this;
            random = new SeededRandom(SaveLoader.Instance.worldDetailSave.globalWorldSeed);
            delay = new MinMax(ModSettings.WorldEvents.MinDelayBetweenEvents, ModSettings.WorldEvents.MaxDelayBetweenEvents);
            SetActivityLevel();

            WorldEvents.Register(OnEventSpawned, OnEventRemoved);
            Subscribe((int)WorldEventHashes.EventEnded, OnEventEnded);

            ConfigureWorldEventInfos();
        }

        private void SetActivityLevel()
        {
            if (ModSettings.WorldEvents.DisableWorldEvents)
                ActivityLevel = Activity.None;
            else if (SaveLoader.Instance.GameInfo.worldTraits.Contains("traits/low_seismic_activity"))
                ActivityLevel = Activity.Low;
            else if (SaveLoader.Instance.GameInfo.worldTraits.Contains("traits/high_seismic_activity"))
                ActivityLevel = Activity.High;
            else ActivityLevel = Activity.Normal;
        }

        private void ConfigureWorldEventInfos()
        {
            worldEventInfos = new List<WorldEventInfo>()
            {
                new WorldEventInfo(
                    EarthQuakeConfig.ID,
                    ModSettings.WorldEvents.EarthQuake.Weight,
                    ModSettings.WorldEvents.EarthQuake.MinDuration,
                    ModSettings.WorldEvents.EarthQuake.MaxDuration)
            };
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            UpdateCurrentEvent();
            CreateRandomEvent();
        }

        private void UpdateCurrentEvent()
        {
            if (WorldEvents != null && WorldEvents.Count > 0)
            {
                WorldEvents.Items.Sort((w1, w2) => w1.StartingIn.CompareTo(w2.StartingIn));
                currentEvent = WorldEvents.Items.First();
            }
            else
            {
                CreateRandomEvent();
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
            foreach (WorldEvent worldevent in WorldEvents)
            {
                worldevent.End();
            }
        }

        private void OnEventRemoved(WorldEvent we)
        {
            Trigger((int)WorldEventHashes.EventEnded, we);
        }

        public void ClearAllEvents()
        {
            for (int i = 0; i < WorldEvents.Count; i++)
            {
                WorldEvents[i].End();
                Destroy(WorldEvents[i].gameObject);
            }
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

        public WorldEvent CreateRandomEvent()
        {
            WorldEventInfo nextEvent = WeightedRandom.Choose(worldEventInfos, random);
            float nextSchedule = delay.GetRandomValueWithinRange(random);

            if (WorldEvents.Count > 0)
                nextSchedule += WorldEvents.Items
                .Where(w => w.immediateStart == false && w.schedule.IsValid)
                .Max(w => w.schedule.TimeRemaining);

            WorldEvent we = Utils.Spawn(nextEvent.prefabID, Vector3.zero)?.GetComponent<WorldEvent>();
            if (we != null)
            {
                we.randomize = true;
                Schedule(we, nextSchedule);
            }

            return we;
        }

        public WorldEvent CreateControlledEvent(string PrefabID, Vector3 location, float time = -1, float power = -1, float duration = -1, bool immediateStart = false)
        {
            Debug.Log("CreateControlledEvent");
            GameObject obj = Utils.Spawn(PrefabID, location);
            if (obj != null && obj.TryGetComponent(out WorldEvent we))
            {
                we.power = power;
                we.immediateStart = immediateStart;
                we.randomize = false;

                if (!immediateStart)
                {
                    Debug.Log("scheduling");
                    Schedule(we, time);
                }

                return we;
            }

            Debug.Log("Failed to spawn");
            return null;
        }

        public static void DestroyInstance() => Instance = null;

        private struct WorldEventInfo : IWeighted
        {
            public string prefabID;
            public float weight { get; set; }
            public MinMax duration;

            public WorldEventInfo(string prefabID, float weight, float min, float max)
            {
                this.prefabID = prefabID;
                this.weight = weight;
                duration = new MinMax(min, max);
            }
        }

        public enum Activity
        {
            None,
            Low,
            Normal,
            High,
            Absurd
        }

#if DEBUG
        // Displays a small UI showing Debug information
        Vector2 scrollPosition;
        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 200, 200, 500));
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(200), GUILayout.Height(500));

            GUILayout.Box("World Events");
            if (GUILayout.Button("Remove all"))
                ClearAllEvents();

            if (GUILayout.Button("Create Random Event"))
                CreateRandomEvent();

            foreach (WorldEvent ev in WorldEvents)
            {
                string text = "<b>" + ev.GetProperName() + "</b>";
                text += "Power: " + ev.power + "\n";

                if (ev is EarthQuake)
                    text += (ev as EarthQuake).mainWave ? "Main\n" : "Aftershock\n";


                switch (ev.Stage)
                {
                    case WorldEvent.WorldEventStage.Spawned:
                        text += $"<color=yellow><b>Starting in: {ev.schedule.TimeRemaining}</b></color>";
                        break;
                    case WorldEvent.WorldEventStage.Active:
                        text += "<color=red><b>Active</b></color>\n";
                        break;
                    case WorldEvent.WorldEventStage.Finished:
                    default:
                        text += "<color=grey><b>Finished</b></color>\n";
                        break;
                }

                GUILayout.Label(text);

                if (GUILayout.Button("Find"))
                    CameraController.Instance.CameraGoTo(ev.transform.position);

                GUILayout.Space(20);
            }

            GUILayout.EndArea();
            GUILayout.EndScrollView();
        }
#endif
    }
}
