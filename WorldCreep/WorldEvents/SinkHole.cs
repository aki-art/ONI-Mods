using KSerialization;
using UnityEngine;

namespace WorldCreep.WorldEvents
{
    public class SinkHole : WorldEvent
    {
        const float RADIUS_MULTIPLIER = 20f;
        const float MAGNITUDE_BIAS = 0.45f;

        [Serialize] public int radius;

        private float MinPower => ModSettings.WorldEvents.SinkHole.MinPower;
        private float MaxPower => Mathf.Min(ModSettings.WorldEvents.SinkHole.MaxPower, SeismicGrid.highestActivity);

        public void Randomize()
        {
            if (randomize)
            {
                float powerRange = MaxPower - MinPower;
                power = Util.Bias(Random.value, MAGNITUDE_BIAS) / powerRange + MinPower;
                power = Mathf.Clamp(power, MinPower, MaxPower);
                radius = Mathf.FloorToInt(Power * RADIUS_MULTIPLIER);

                transform.position = Grid.CellToPos(SeismicGrid.GetRandomCell(true));
            }
        }

        protected override void OnSpawn() // gets called when this object spawns  
        {
            Randomize();
            affectedCells = SeismicGrid.GetCircle(this, radius, 0);
            base.OnSpawn();
        }

        public override void Begin()
        {
            base.Begin();
            Debug.Log("fwump");
        }
    }
}
