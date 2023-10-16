using Moonlet.Templates.SubTemplates;
using ProcGen;
using System.Collections.Generic;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class SubworldTemplate : ITemplate
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Priority { get; set; }
		public Dictionary<string, string> PriorityPerCluster { get; set; }
		public string Description { get; set; }
		public string UtilityDescription { get; set; }
		public string BiomeNoise { get; set; }
		public string OverrideNoise { get; set; }
		public string DensityNoise { get; set; }
		public string BorderOverride { get; set; }
		public IntNumber BorderOverridePriority { get; set; }
		public MinMax BorderSizeOverride { get; set; }
		public string TemperatureRange { get; set; }
		public Feature CentralFeature { get; set; }
		public List<Feature> Features { get; set; }
		public SampleDescriber.Override Overrides { get; set; } // TODO
		public List<string> Tags { get; set; }
		public IntNumber MinChildCount { get; set; }
		public bool SingleChildCount { get; set; }
		public IntNumber ExtraBiomeChildren { get; set; }
		public List<WeightedBiome> Biomes { get; set; }
		public Dictionary<string, int> FeatureTemplates { get; set; }
		public List<TemplateSpawnRuleC> SubworldTemplateRules { get; set; }
		public IntNumber Iterations { get; set; }
		public FloatNumber MinEnergy { get; set; }
		public string ZoneType { get; set; }
		public List<SampleDescriber> Samplers { get; set; }
		public FloatNumber PdWeight { get; set; }
	}
}
