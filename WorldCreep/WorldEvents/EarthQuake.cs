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

                spawnsGeyser = SeismicGrid.FindAppropiateEpicenter(Power, Tuning.WorldEvent.GEYSER_TRESHOLD, out int epicenter);
                spawnsGeyser &= (bool)ModSettings.WorldEvents.EarthQuake.CanSpawnGeyser;
                transform.position = Grid.CellToPos(epicenter);
            }

            radius = Mathf.FloorToInt(Power * RADIUS_MULTIPLIER);
        }



        protected override void Initialize() 
        {
            //gameObject.AddComponent<EventVisualizer>();
            //gameObject.DrawCircle(5, 0.2f);
            visualizer = gameObject.AddComponent<SeismicEventVisualizer>();
            visualizer.redBandDistance = 3;
            visualizer.yellowbandDistance = 7;

            Randomize();

            if(radius <= 0)
            {
                Log.Warning("Invalid setting on Earthquake, radius cannot be " + radius);
                return;
            }

            int falloff = Mathf.FloorToInt(radius * 0.25f);
            affectedCells = SeismicGrid.GetCircle(this, radius - falloff, falloff);
        }

        public override void Begin()
        {
            base.Begin();
            Debug.Log("rumble rumble");
        }
    }
}
