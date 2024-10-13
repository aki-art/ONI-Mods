extern alias YamlDotNetButNew;

using Moonlet.Utils;
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

		public Dictionary<string, Dictionary<string, object>> ModData { get; set; }
		public bool Optional { get; set; }
		public Conditions Conditions { get; set; }
	}
}
