extern alias YamlDotNetButNew;
using ProcGen;
using System.Collections.Generic;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class BiomeTemplate : Dictionary<string, ComposableDictionary<string, ElementBandConfiguration>>, ITemplate
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Priority { get; set; }
		public ITemplate.MergeBehavior Command { get; set; }

		// ComposableDictionary<string, ElementBandConfiguration>> TerrainBiomeLookupTable { get; set; }

		// handle inconsistent casing for key
		public ComposableDictionary<string, ElementBandConfiguration> GetValue()
		{
			if (TryGetValue("TerrainBiomeLookupTable", out var kleiStyle))
				return kleiStyle;

			if (TryGetValue("terrainBiomeLookupTable", out var consistentCasing))
				return consistentCasing;

			return null;
		}

		[YamlIgnore] public Dictionary<string, string> PriorityPerClusterTag { get; set; }

		Dictionary<string, object> ITemplate.Conditions { get; set; }
	}
}
