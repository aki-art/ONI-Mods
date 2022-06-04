using FUtility;
using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TransparentAluminum.Content.Buildings.SolarRoad
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class SolarRoad : Generator
    {
        [SerializeField]
        public List<Tier> tiers = new List<Tier>();
        [Serialize]
        public int currentTier;

        private int cellAbove;
        private int maxTier;

        public const float W_P_LUX = 0.00053f / 8f;
        public const float BASE_WATTS = 32f;

        //public new float BaseWattageRating => 20f;

        private HandleVector<int>.Handle accumulator = HandleVector<int>.InvalidHandle;

        public float GetMaxWatts => 400f; //tiers[currentTier].maxWatts;

        public float GetWattPerLux => W_P_LUX;// tiers[currentTier].wattsPerLux;

        public bool IsMaxLevel => currentTier == maxTier;

        protected override void OnSpawn()
        {
            base.OnSpawn();

            cellAbove = Grid.CellAbove(Grid.PosToCell(this));
            accumulator = Game.Instance.accumulators.Add("SolarRoadLux", this);
            maxTier = tiers.Count - 1;

            Subscribe((int)ModHashes.OnBuildingUpgraded, OnBuildingUpgraded);

            SetTier(currentTier);
        }

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            tiers.Add(new SolarRoad.Tier(STRINGS.BUILDINGS.PREFABS.TRANSPARENTALUMINUM_SOLARROAD.TIERS.TIER1, 0, Color.gray));
            tiers.Add(new SolarRoad.Tier(STRINGS.BUILDINGS.PREFABS.TRANSPARENTALUMINUM_SOLARROAD.TIERS.TIER2, 1, Color.cyan));
            tiers.Add(new SolarRoad.Tier(STRINGS.BUILDINGS.PREFABS.TRANSPARENTALUMINUM_SOLARROAD.TIERS.TIER3, 2, Color.yellow));
            tiers.Add(new SolarRoad.Tier(STRINGS.BUILDINGS.PREFABS.TRANSPARENTALUMINUM_SOLARROAD.TIERS.TIER4, 3, Color.green));

            Attributes attributes = base.gameObject.GetAttributes();

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

            var lux = GetLuxAbove();

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
            return Grid.LightIntensity[cellAbove] * GetWattPerLux;
        }

        private void OnBuildingUpgraded(object obj)
        {
            if(obj is int level)
            {
                SetTier(level);
            }
        }

        public void SetTier(int tier)
        {
            Log.Debuglog("SETTING TIER TO " + tier);
            Log.Debuglog("tiers: " + tiers?.Count);

            var attributes = gameObject.GetAttributes();
            attributes.Remove(tiers[currentTier].mod);
            attributes.Add(tiers[tier].mod);

            currentTier = tier;

            GetComponent<KBatchedAnimController>().TintColour = tiers[tier].color;
        }

        [Serializable]
        public class Tier
        {
            public string name;
            public float wattsPerLux;
            public float maxWatts;
            public Color color;

            public AttributeModifier mod;

            public Tier(string name, int tier, Color color)
            {
                var multiplier = Mathf.Pow(2, tier);

                this.name = name;
                wattsPerLux = W_P_LUX * multiplier;
                maxWatts = BASE_WATTS * multiplier;
                this.color = color;

                var invTier = 4 - tier;
                var tierModifier = invTier / 4f;

                var maximum = 8f;
                var percent = multiplier / maximum;
                percent *= 100f;
                percent -= 100f;

                mod = new AttributeModifier(Db.Get().Attributes.GeneratorOutput.Id, percent, "mod");
            }

            public override string ToString()
            {
                return $"Tier: {name} - {wattsPerLux}W/lux, {maxWatts} max W";
            }
        }
    }
}
