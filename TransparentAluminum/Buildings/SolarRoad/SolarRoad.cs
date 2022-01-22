using KSerialization;
using System.Collections.Generic;
using UnityEngine;
using static TransparentAluminum.STRINGS.BUILDINGS.PREFABS.TRANSPARENTALUMINUM_SOLARROAD;

namespace TransparentAluminum.Buildings.SolarRoad
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class SolarRoad : Generator, ISaveLoadable
    {
        [MyCmpReq]
        Upgradeable upgradeable;

        private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

        private const float W_P_LUX = 0.00053f / 8f;

        public float GetMaxWatts => tiers[upgradeable.currentLevel].maxWatts;
        public float GetWattPerLux => tiers[upgradeable.currentLevel].wattsPerLux;

        private static List<Tier> tiers = new List<Tier>()
        {
            new Tier("rickety", TIERS.TIER1, W_P_LUX, 32),
            new Tier("reinforced", TIERS.TIER2, W_P_LUX * 2, 64),
            new Tier("advanced", TIERS.TIER3, W_P_LUX * 4, 128),
            new Tier("hi-tech", TIERS.TIER4, W_P_LUX * 8, 256)
        };

        protected override void OnSpawn()
        {
            base.OnSpawn();
            //Subscribe((int)GameHashes.ActiveChanged, OnActiveChanged);
            accumulator = Game.Instance.accumulators.Add("Element", this);
        }

        protected override void OnCleanUp()
        {
            Game.Instance.accumulators.Remove(accumulator);
            base.OnCleanUp();
        }

        public override void EnergySim200ms(float dt)
        {
            base.EnergySim200ms(dt);

            operational.SetFlag(wireConnectedFlag, CircuitID != ushort.MaxValue);

            if (!operational.IsOperational)
            {
                return;
            }

            float lux = GetLuxAbove();
            operational.SetActive(lux > 0f);

            lux = Mathf.Clamp(lux, 0f, GetMaxWatts);

            Game.Instance.accumulators.Accumulate(accumulator, lux * dt);

            if (lux > 0f)
            {
                lux *= dt;
                lux = Mathf.Max(lux, dt);
                GenerateJoules(lux);
            }

            //meter.SetPositionPercent(Game.Instance.accumulators.GetAverageRate(accumulator) / 380f);
            //UpdateStatusItem();
        }

        private float GetLuxAbove()
        {
            return Grid.LightIntensity[Grid.CellAbove(Grid.PosToCell(this))] * GetWattPerLux;
        }

        public class Tier
        {
            public string id;
            public string name;
            public float wattsPerLux;
            public float maxWatts;

            public Tier(string id, string name, float wattsPerLux, float maxWatts)
            {
                this.id = id;
                this.name = name;
                this.wattsPerLux = wattsPerLux;
                this.maxWatts = maxWatts;
            }
        }
    }
}
