using KSerialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorldCreep.WorldEvents
{
    public class EarthQuake : WorldEvent
    {
        const float RADIUS_MULTIPLIER = 50f;
        const float MAGNITUDE_BIAS = 0.2f;
        const float AFTERSHOCK_MIN_TRESHOLD = 0.2f;
        const float AFTERSHOCK_DISTANCE_RANGE = 1.2f;
        const float AFTERSHOCK_MIN_DELAY = 10f;
        const float AFTERSHOCK_MAX_DELAY = 30f;

        [Serialize]
        public bool mainWave;

        public bool SpawnsGeyser => (bool)ModSettings.WorldEvents.EarthQuake.CanSpawnGeyser && Power >= Tuning.WorldEvent.GEYSER_TRESHOLD;
        private float MinPower => ModSettings.WorldEvents.EarthQuake.MinPower;
        private float MaxPower => Mathf.Min(ModSettings.WorldEvents.EarthQuake.MaxPower, SeismicGrid.highestActivity);

        public override float Power
        {
            get => power;
            set => power = Mathf.Clamp(value, MinPower, MaxPower);
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            mainWave = true;
        }

        protected override void Initialize()
        {
            SetVisualizer();
            Randomize();
            SetCells();
            if (mainWave && Stage == WorldEventStage.Spawned)
            {
               // CreateAfterShocks();
            }
        }

        public void Randomize()
        {
            durationInSeconds = 50f;
            if (randomize)
            {
                float powerRange = MaxPower - MinPower;
                power = Util.Bias(Random.value, MAGNITUDE_BIAS) / powerRange + MinPower;
                power = Mathf.Clamp(power, MinPower, MaxPower);

                SeismicGrid.FindAppropiateEpicenter(Power, Tuning.WorldEvent.GEYSER_TRESHOLD, out int epicenter);
                transform.position = Grid.CellToPos(epicenter);
            }

            radius = Mathf.FloorToInt(Power * RADIUS_MULTIPLIER);
        }

        public override void Begin()
        {
            base.Begin();
            StartCoroutine(Loop());
            // Shake Camera
            // Crush Blocks
            // Damage Buildings
            // Move liquids?
            // Spawn Geyser
        }


        private IEnumerator Loop()
        {
            PerlinNoise perlin = new PerlinNoise(Random.Range(1, 999));
            float progress;

            while (Stage != WorldEventStage.Finished)
            {
                elapsedTime += Time.deltaTime;
                progress = elapsedTime / durationInSeconds;

                if (progress < 1)
                {
                    ShakeCamera(perlin, progress);
                    //soundEventInstance.setVolume(2 * t);
                    //effectivePower = power * t;
                    // vignette.SetIntensity(t);

                    yield return new WaitForFixedUpdate();
                }
                else
                {
                    End();
                }
            }

            yield return null;
        }

        private void ShakeCamera(PerlinNoise perlin, float progress)
        {
            Vector3 currentPos = CameraController.Instance.transform.GetPosition();
            float distance = Vector2.Distance(currentPos, transform.position);
            //if (distance > radius) return;

            float noiseScale = (1f - distance / radius) / 2f;
            float xScale = 1.5f;
            float frequency = 7;

            float t = 1f; // progress < 0.5f ? progress * 2 : (1 - progress) * 2;
            float x = (float)perlin.Noise(elapsedTime * frequency * xScale, 0, 0);
            float y = (float)perlin.Noise(0, elapsedTime * frequency, 0);

            Vector3 offset = new Vector3(x, y) * t * noiseScale;

            CameraController.Instance.SetPosition(currentPos + offset);
        }

        public EarthQuake CreateAfterShock()
        {
            Vector3 location = GetNearbySpot();
            float power = Mathf.Min(Power * Random.Range(0.1f, 0.6f), SeismicGrid.activity[Grid.PosToCell(location)]);
            float time = (immediateStart ? 0 : schedule.TimeRemaining) + Random.Range(AFTERSHOCK_MIN_DELAY, AFTERSHOCK_MAX_DELAY);
            return WorldEventScheduler.Instance.CreateControlledEvent(
                EarthQuakeConfig.ID, location, time, power) as EarthQuake;
        }

        private Vector3 GetNearbySpot()
        {
            Vector3 result;
            int cell;
            do
            {
                result = (Vector3)Random.insideUnitCircle * radius * AFTERSHOCK_DISTANCE_RANGE + transform.position;
                cell = Grid.PosToCell(result);
            } while (!(Grid.IsValidCell(cell) && SeismicGrid.activity[cell] > 0));

            return result;
        }

        private void SetVisualizer()
        {
            visualizer = gameObject.AddComponent<SeismicEventVisualizer>();
            visualizer.redBandDistance = 3;
            visualizer.yellowbandDistance = 7;
        }

        private void SetCells()
        {
            int falloff = Mathf.FloorToInt(radius * 0.25f);
            affectedCells = SeismicGrid.GetCircle(this, radius - falloff, falloff);
        }

        private void CreateAfterShocks()
        {
            if (Power >= AFTERSHOCK_MIN_TRESHOLD)
            {
                for (int i = 0; i <= Random.Range(0, Mathf.Floor(Power * 7)); i++)
                {
                    CreateAfterShock().mainWave = false;
                }
            }
        }

    }


}
