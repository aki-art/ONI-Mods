using FUtility;
using KSerialization;
using UnityEngine;

namespace WorldCreep.WorldEvents
{
    public class EarthQuake : WorldEvent
    {
        const float RADIUS_MULTIPLIER = 50f;
        const float MAGNITUDE_BIAS = 0.45f;

        [Serialize] 
        public int radius;
        [Serialize] 
        private bool spawnsGeyser;

        private float MinPower => ModSettings.WorldEvents.EarthQuake.MinPower;
        private float MaxPower => Mathf.Min(ModSettings.WorldEvents.EarthQuake.MaxPower, SeismicGrid.highestActivity);

        public void Randomize()
        {
            if (randomize)
            {
                float powerRange = MaxPower - MinPower;
                power = Util.Bias(Random.value, MAGNITUDE_BIAS) / powerRange + MinPower;
                power = Mathf.Clamp(power, MinPower, MaxPower);

                radius = Mathf.FloorToInt(Power * RADIUS_MULTIPLIER);

                spawnsGeyser = SeismicGrid.FindAppropiateEpicenter(Power, Tuning.WorldEvent.GEYSER_TRESHOLD, out int epicenter);
                spawnsGeyser &= (bool)ModSettings.WorldEvents.EarthQuake.CanSpawnGeyser;
                transform.position = Grid.CellToPos(epicenter);
            }
        }

        protected override void OnSpawn() // gets called when this object spawns  
        {
            Randomize();

            if(radius <= 0)
            {
                Log.Warning("Invalid setting on Earthquake, radius cannot be " + radius);
                return;
            }

            int falloff = Mathf.FloorToInt(radius * 0.25f);
            affectedCells = SeismicGrid.GetCircle(this, radius - falloff, falloff);

            base.OnSpawn();
        }

        public override void Begin()
        {
            base.Begin();
            Debug.Log("rumble rumble");
        }
    }
}
