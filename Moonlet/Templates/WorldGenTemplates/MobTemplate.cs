extern alias YamlDotNetButNew;
using ProcGen;
using System.Collections.Generic;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class MobTemplate : Dictionary<string, ComposableDictionary<string, Mob>>, ITemplate
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Priority { get; set; }

		// handle inconsistent casing for key
		public ComposableDictionary<string, Mob> GetValue()
		{
			if (TryGetValue("MobLookupTable", out var kleiStyle))
				return kleiStyle;

			if (TryGetValue("mobLookupTable", out var consistentCasing))
				return consistentCasing;

			return null;
		}

		[YamlIgnore]
		public Dictionary<string, string> PriorityPerCluster { get; set; }
	}
}
