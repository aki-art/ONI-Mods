using Moonlet.Templates.SubTemplates;
using ProcGen;
using System.Collections.Generic;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class SubworldTemplate : ITemplate
	{
		// Custom
		public string Category { get; set; }
		public string Id { get; set; }
		public string Name { get; set; }
		public string Priority { get; set; }
		public Dictionary<string, string> PriorityPerClusterTag { get; set; }
		public ITemplate.MergeBehavior Command { get; set; }

		// SampleDescriber ----------------------------------------------------------------------------------------
		public SampleDescriber.PointSelectionMethod SelectMethod { get; set; }
		public MinMax Density { get; set; }
		public float AvoidRadius { get; set; }
		public PointGenerator.SampleBehaviour SampleBehaviour { get; set; }
		public bool DoAvoidPoints { get; set; }
		public bool DontRelaxChildren { get; set; }
		public MinMax BlobSize { get; set; }

		// Subworld ------------------------------------------------------------------------------------------------
		//public string Description { get; set; } // unused
		//public string UtilityDescription { get; set; } // unused
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
