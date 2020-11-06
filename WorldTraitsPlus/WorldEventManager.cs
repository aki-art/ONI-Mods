using FUtility;
using Harmony;
using ProcGen;
using System.Collections.Generic;
using UnityEngine;
using WorldTraitsPlus.WorldEvents;
using static ProcGen.SubWorld;

namespace WorldTraitsPlus
{

    public class WorldEventManager : KMonoBehaviour
    {
        public SeededRandom random;
        public WorldEvent currentEvent;
        public Dictionary<Event, EventInfo> eventInfos;
        public List<WorldEvent> activeEvents;
        public SchedulerHandle schedule;
        public Dictionary<int, ZoneType> zoneTiles;

        public float TimeUntilNextEvent => schedule.TimeRemaining;
        public static WorldEventManager Instance { get; private set; }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            if (currentEvent == null)
                ScheduleNext();

            Subscribe((int)WorldEventHashes.WorldEventEnded, OnWorldEventEnded);
            Subscribe((int)WorldEventHashes.WorldEventStarted, OnWorldEventStarted);
        }

        private void OnWorldEventStarted(object obj)
        {
            Debug.Log("WorldEventHashes: event has started");
        }

        protected override void OnPrefabInit()
        {
            Instance = this;
            activeEvents = new List<WorldEvent>();
            zoneTiles = new Dictionary<int, ZoneType>();
            eventInfos = new Dictionary<Event, EventInfo>
            {
                { Event.Earthquake, new EventInfo(STRINGS.DISASTERS.EARTHQUAKE.NAME, ModAssets.settings.EarthquakeDuration, EarthQuakeConfig.ID, 0f) },
                { Event.Sinkhole, new EventInfo(STRINGS.DISASTERS.EARTHQUAKE.NAME, ModAssets.settings.EarthquakeDuration, SinkHoleConfig.ID, 1f) }
            };

        }

        private void OnWorldEventEnded(object obj)
        {
            currentEvent = null;
            Debug.Log("world event ended \\o/");
        }

        public void ScheduleNext()
        {
            float nextSchedule = ModAssets.settings.WorldEventDelay.Get();
            //SpawnEvent(Event.Sinkhole);
            CreateWorldEvent(NextEvent(), 0.9f, false, TestLocation(), true);
            schedule = GameScheduler.Instance.Schedule("Begin" + currentEvent.ToString(), nextSchedule, obj => currentEvent.Begin());
            Log.Debuglog($"Scheduled world event ({currentEvent.ToString()}), starting in {TimeUntilNextEvent}");
        }

        public virtual void CancelCurrent()
        {
            Log.Info("Cancelled worldevent: " + currentEvent.GetProperName());
            schedule.ClearScheduler();
            currentEvent = null;
        }

        public virtual void CancelAllEvents(bool preventSchedule)
        {
            schedule.ClearScheduler();
            currentEvent = null;
            activeEvents.ForEach(e => e.End(false));
            activeEvents.Clear();
            Log.Info("Cancelled all world events.");

            if(!preventSchedule)
            {
                ScheduleNext();
            }
        }

        private void CreateWorldEvent(Event ev, float power, bool randomizeLocation, Vector3 position, bool schedule = true)
        {
            if(eventInfos.TryGetValue(ev, out EventInfo info))
            {
                var prefab = Assets.GetPrefab(info.prefab);
                GameObject eq = GameUtil.KInstantiate(prefab, position, Grid.SceneLayer.Creatures);
                WorldEvent newEvent = eq.GetComponent<WorldEvent>();
                newEvent.power = power;
                newEvent.randomizeLocation = randomizeLocation;

                if (schedule)
                {
                    this.schedule.ClearScheduler();
                    currentEvent = newEvent;
                }

                eq.SetActive(true);
                Log.Debuglog("Spawned event " + eq.GetProperName());
            }
        }

        public void Initialize()
        {
            random = new SeededRandom(SaveLoader.Instance.worldDetailSave.globalWorldSeed);

            SeismicGrid.Initialize();
            gameObject.SetActive(true);

            Subscribe((int)WorldEventHashes.WorldEventEnded, OnWorldEventEnded);
        }

        private Vector3 TestLocation() => Grid.CellToPos(Grid.XYToCell(Grid.WidthInCells / 2, Grid.HeightInCells / 2));

        public void SpawnEvent(Event name)
        {

            EventInfo info = eventInfos[name];
            var prefab = Assets.GetPrefab(info.prefab);

            GameObject eq = GameUtil.KInstantiate(prefab, TestLocation(), Grid.SceneLayer.Creatures);
            currentEvent = eq.GetComponent<WorldEvent>();
            currentEvent.power = 0.9f;
            currentEvent.randomizeLocation = false;
            eq.SetActive(true);

        }

        public static void DestroyInstance() => Instance = null;

        public enum Event
        {
            AuroraBorealis,
            Drought,
            Earthquake,
            Eclipse,
            MegaMeteor,
            SolarFlare,
            SpontaneousEruptions,
            Sinkhole
        }

        private Event NextEvent()
        {
            return eventInfos.ChooseWeighted(random);
        }

        public void SetBackgroundWall(List<int> cells, ZoneType zoneType)
        {
            foreach (int cell in cells)
            {
                SimMessages.ModifyCellWorldZone(cell, byte.MaxValue);
                zoneTiles[cell] = zoneType;
            }

            RegenerateBackwallTexture();
        }

        private void RegenerateBackwallTexture()
        {
            if(World.Instance.zoneRenderData == null)
            {
                Debug.Log("Subworld zone render data is not yet initialized.");
                return;
            }

            var zoneRenderData = Traverse.Create(World.Instance.zoneRenderData);
            var colourTexField = zoneRenderData.Field("colourTex");
            var indexTexField = zoneRenderData.Field("indexTex");

            // Copy the textures of the backwall
            Texture2D colourTex = colourTexField.GetValue<Texture2D>();
            Texture2D indexTex = indexTexField.GetValue<Texture2D>();

            byte[] zoneIndices = colourTex.GetRawTextureData();
            byte[] colors = indexTex.GetRawTextureData();

            foreach (var tile in zoneTiles)
            {
                Color32 color = World.Instance.zoneRenderData.zoneColours[(int)tile.Value];
                int cell = tile.Key;
                colors[cell] = (tile.Value == ZoneType.Space) ? byte.MaxValue : (byte)tile.Value;
                zoneIndices[cell * 3] = color.r;
                zoneIndices[cell * 3 + 1] = color.g;
                zoneIndices[cell * 3 + 2] = color.b;
                World.Instance.zoneRenderData.worldZoneTypes[cell] = tile.Value;
            }

            colourTex.LoadRawTextureData(zoneIndices);
            indexTex.LoadRawTextureData(colors);
            colourTex.Apply();
            indexTex.Apply();

            colourTexField.SetValue(colourTex);
            indexTexField.SetValue(indexTex);

            zoneRenderData.Method("OnShadersReloaded").GetValue();
            //zoneRenderData.Method("InitSimZones", colors).GetValue();
        }

        public struct EventInfo : IWeighted
        {
            public string name;
            public MathUtil.MinMax duration;
            public string prefab;
            public float weight { get; set; }

            public EventInfo(string name, MathUtil.MinMax duration, string prefab, float weight)
            {
                this.name = name;
                this.duration = duration;
                this.prefab = prefab;
                this.weight = weight;
            }
        }
    }
}
