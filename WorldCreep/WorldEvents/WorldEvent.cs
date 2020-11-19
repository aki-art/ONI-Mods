using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace WorldCreep.WorldEvents
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class WorldEvent : KMonoBehaviour, ISaveLoadable
    {
        // TODO: serialize less, most of this can be inferred
        [SerializeField]
        public bool randomize = true;
        [Serialize]
        public string test;
        [SerializeField]
        [Serialize]
        public bool immediateStart;
        [SerializeField]
        [Serialize]
        public float power;
        [SerializeField]
        [Serialize]
        public float durationInSeconds;
        [SerializeField]
        public Dictionary<int, float> affectedCells;
        public SchedulerHandle schedule;
        [Serialize]
        public int radius;
        [Serialize]
        public bool showOnOverlay;
        [Serialize]
        public float elapsedTime;
        public SeismicEventVisualizer visualizer;

        public float StartingIn => schedule.IsValid ? schedule.TimeRemaining : float.PositiveInfinity;

        public virtual float Power 
        { 
            get => power; 
            set => power = value; 
        }

        public WorldEventStage Stage { get; protected set; }

        public float DurationInCycles
        {
            get => durationInSeconds / 600f;
            set => durationInSeconds = value * 600f;
        }

        public virtual void Begin() 
        {
            Debug.Log("started");
            Stage = WorldEventStage.Active;
            schedule.ClearScheduler();
        }

        public virtual void End()
        {
            Stage = WorldEventStage.Finished;
        }

        protected virtual void Initialize()
        {
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            Initialize();

            WorldEventScheduler.Instance.WorldEvents.Add(this);
            Stage = WorldEventStage.Spawned;
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            WorldEventScheduler.Instance.WorldEvents.Remove(this);
            if (schedule.IsValid)
                schedule.ClearScheduler();
        }
        public enum WorldEventStage
        {
            Spawned,
            Active,
            Finished
        }
    }
}
