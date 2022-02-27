using Klei.AI;
using Klei.AI.DiseaseGrowthRules;

namespace Beached.Germs
{
    internal class PlanktonGerms : Disease
    {
        public const string ID = "Beached_Plankton";
        public const float RAD_KILL_RATE = 2.5f;

        private static readonly RangeInfo temperatureRangeInfo = new RangeInfo(283.15f, 293.15f, 363.15f, 373.15f);
        private static readonly RangeInfo temperatureHalfLivesInfo = new RangeInfo(10f, 1200f, 1200f, 10f);
        private static readonly RangeInfo pressureRangeInfo = new RangeInfo(0f, 0f, 1000f, 1000f);

        public PlanktonGerms(bool statsOnly) : base(ID, 20, temperatureRangeInfo, temperatureHalfLivesInfo,
            pressureRangeInfo, RangeInfo.Idempotent(), RAD_KILL_RATE, statsOnly)
        {
        }

        protected override void PopulateElemGrowthInfo()
        {
            InitializeElemGrowthArray(ref elemGrowthInfo, DEFAULT_GROWTH_INFO);

            AddGrowthRule(new GrowthRule
            {
                underPopulationDeathRate = new float?(2.6666667f),
                minCountPerKG = new float?(0.4f),
                populationHalfLife = new float?(12000f),
                maxCountPerKG = new float?((float)500),
                overPopulationHalfLife = new float?(1200f),
                minDiffusionCount = new int?(1000),
                diffusionScale = new float?(0.001f),
                minDiffusionInfestationTickCount = new byte?(1)
            });

            AddGrowthRule(new StateGrowthRule(Element.State.Solid)
            {
                minCountPerKG = new float?(0.4f),
                populationHalfLife = new float?(3000f),
                overPopulationHalfLife = new float?(1200f),
                diffusionScale = new float?(1E-06f),
                minDiffusionCount = new int?(1000000)
            });

            AddGrowthRule(new ElementGrowthRule(Elements.Mucus)
            {
                underPopulationDeathRate = new float?(0f),
                populationHalfLife = new float?(-3000f),
                overPopulationHalfLife = new float?(3000f),
                maxCountPerKG = new float?((float)4500),
                diffusionScale = new float?(0.05f)
            });

            AddGrowthRule(new ElementGrowthRule(SimHashes.BleachStone)
            {
                populationHalfLife = new float?(10f),
                overPopulationHalfLife = new float?(10f),
                minDiffusionCount = new int?(100000),
                diffusionScale = new float?(0.001f)
            });

            AddGrowthRule(new StateGrowthRule(Element.State.Gas)
            {
                minCountPerKG = new float?(0.4f),
                populationHalfLife = new float?(3000f),
                overPopulationHalfLife = new float?(1200f),
                diffusionScale = new float?(1E-06f),
                minDiffusionCount = new int?(1000000)
            });

            AddGrowthRule(new StateGrowthRule(Element.State.Liquid)
            {
                minCountPerKG = new float?(0.4f),
                populationHalfLife = new float?(1200f),
                overPopulationHalfLife = new float?(300f),
                maxCountPerKG = new float?((float)100),
                diffusionScale = new float?(0.01f)
            });

            AddGrowthRule(new ElementGrowthRule(SimHashes.Brine)
            {
                populationHalfLife = new float?(500f),
                overPopulationHalfLife = new float?(60f),
                minDiffusionCount = new int?(100000),
                diffusionScale = new float?(0.001f)
            });

            InitializeElemExposureArray(ref elemExposureInfo, DEFAULT_EXPOSURE_INFO);

            AddExposureRule(new ExposureRule
            {
                populationHalfLife = new float?(float.PositiveInfinity)
            });

            AddExposureRule(new ElementExposureRule(SimHashes.SaltWater)
            {
                populationHalfLife = new float?(-12000f)
            });

            AddExposureRule(new ElementExposureRule(SimHashes.Oxygen)
            {
                populationHalfLife = new float?(3000f)
            });

            AddExposureRule(new ElementExposureRule(SimHashes.ChlorineGas)
            {
                populationHalfLife = new float?(10f)
            });
        }
    }
}
