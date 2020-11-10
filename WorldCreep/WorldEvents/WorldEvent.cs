using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace WorldCreep.WorldEvents
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class WorldEvent : KMonoBehaviour, ISaveLoadable
    {
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

        public float StartingIn => schedule.IsValid ? schedule.TimeRemaining : float.PositiveInfinity;

        public virtual float Power 
        { 
            get => power; 
            set => power = value; 
        }

        public WorldEventStage Stage { get; private set; }

        public float DurationInCycles
        {
            get => durationInSeconds / 600f;
            set => durationInSeconds = value * 600f;
        }

        public virtual void Begin() 
        {
            Stage = WorldEventStage.Ongoing;
            if (affectedCells != null && affectedCells.Count > 0)
            {
                if(!immediateStart)
                {
                    SeismicGrid.UnRegisterUpcomingEvent(affectedCells);
                }

                SeismicGrid.RegisterActiveEvent(affectedCells);
            }

        }

        public virtual void End()
        {
            Stage = WorldEventStage.Finished; 
            if (affectedCells != null && affectedCells.Count > 0)
            {
                SeismicGrid.UnRegisterActiveEvent(affectedCells);
            }
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            WorldEventScheduler.Instance.WorldEvents.Add(this);
            Stage = WorldEventStage.Waiting;

            if(affectedCells != null && affectedCells.Count > 0 && !immediateStart)
            {
                SeismicGrid.RegisterUpcomingEvent(affectedCells);
            }
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
            WorldEventScheduler.Instance.WorldEvents.Remove(this);
        }
        public enum WorldEventStage
        {
            Waiting,
            Ongoing,
            Finished
        }
    }
}
