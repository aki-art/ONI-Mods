using FUtility;
using KSerialization;
using UnityEngine;

namespace WorldCreep.WorldEvents
{
    public class EarthQuake : WorldEvent
    {
        const float GEYSER_TRESHOLD = 0.8f;
        const float RADIUS_MULTIPLIER = 50f;
        const float MAGNITUDE_BIAS = 0.45f;

        [Serialize] public int radius;
        [Serialize] private bool spawnsGeyser;

        private float MinPower => ModSettings.WorldEvents.EarthQuake.MinPower;
        private float MaxPower => Mathf.Min(ModSettings.WorldEvents.EarthQuake.MaxPower, SeismicGrid.highestActivity);

        public void Randomize()
        {
            if (randomize)
            {
                float powerRange = MaxPower - MinPower;
                power = Util.Bias(Random.value, MAGNITUDE_BIAS) / powerRange + MinPower;
                power = Mathf.Clamp(power, MinPower, MaxPower);
                Debug.Log("radomized power to " + power);
                radius = Mathf.FloorToInt(Power * RADIUS_MULTIPLIER);

                spawnsGeyser = SeismicGrid.FindAppropiateEpicenter(Power, GEYSER_TRESHOLD, out int epicenter);
                transform.position = Grid.CellToPos(epicenter);
                spawnsGeyser &= (bool)ModSettings.WorldEvents.EarthQuake.CanSpawnGeyser;
            }
        }



        protected override void OnSpawn()
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
