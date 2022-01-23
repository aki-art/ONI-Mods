using ProcGen;
using System.Collections.Generic;

namespace Terraformer
{
    public class SpacePOIOptions : IWeighted
    {
        public float weight { get; set; }

        public string ID { get; set; }

        public SpacePOIOptions(string ID, float weight = 1f)
        {
            this.ID = ID;
            this.weight = weight;
        }

        public static List<SpacePOIOptions> options = new List<SpacePOIOptions>()
        {
            new SpacePOIOptions(HarvestablePOIConfig.CarbonAsteroidField),
            new SpacePOIOptions(HarvestablePOIConfig.ChlorineCloud),
            new SpacePOIOptions(HarvestablePOIConfig.ForestyOreField),
            new SpacePOIOptions(HarvestablePOIConfig.FrozenOreField),
            new SpacePOIOptions(HarvestablePOIConfig.GasGiantCloud),
            new SpacePOIOptions(HarvestablePOIConfig.GildedAsteroidField),
            new SpacePOIOptions(HarvestablePOIConfig.GlimmeringAsteroidField),
            new SpacePOIOptions(HarvestablePOIConfig.HeliumCloud),
            new SpacePOIOptions(HarvestablePOIConfig.IceAsteroidField),
            new SpacePOIOptions(HarvestablePOIConfig.InterstellarIceField),
            new SpacePOIOptions(HarvestablePOIConfig.InterstellarOcean),
            new SpacePOIOptions(HarvestablePOIConfig.MetallicAsteroidField),
            new SpacePOIOptions(HarvestablePOIConfig.OilyAsteroidField),
            new SpacePOIOptions(HarvestablePOIConfig.OrganicMassField),
            new SpacePOIOptions(HarvestablePOIConfig.OxidizedAsteroidField),
            new SpacePOIOptions(HarvestablePOIConfig.OxygenRichAsteroidField),
            new SpacePOIOptions(HarvestablePOIConfig.RadioactiveAsteroidField),
            new SpacePOIOptions(HarvestablePOIConfig.RadioactiveGasCloud),
            new SpacePOIOptions(HarvestablePOIConfig.RockyAsteroidField),
            new SpacePOIOptions(HarvestablePOIConfig.SaltyAsteroidField),
            new SpacePOIOptions(HarvestablePOIConfig.SandyOreField),
            new SpacePOIOptions(HarvestablePOIConfig.SatelliteField),
            new SpacePOIOptions(HarvestablePOIConfig.SwampyOreField),
        };

        public static string GetRandom() => "HarvestableSpacePOI_" + options.GetRandom().ID;
    }
}
