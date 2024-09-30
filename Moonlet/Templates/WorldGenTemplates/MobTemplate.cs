﻿extern alias YamlDotNetButNew;

using Moonlet.Templates.SubTemplates;
using ProcGen;
using System.Collections.Generic;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class MobTemplate : Dictionary<string, ComposableDictionary<string, MobC>>, ITemplate
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Priority { get; set; }
		public ITemplate.MergeBehavior Command { get; set; }

		// handle inconsistent casing for key
		public ComposableDictionary<string, MobC> GetValue()
		{
			if (TryGetValue("MobLookupTable", out var kleiStyle))
				return kleiStyle;

			if (TryGetValue("mobLookupTable", out var consistentCasing))
				return consistentCasing;

			return null;
		}

		[YamlIgnore]
		public Dictionary<string, string> PriorityPerClusterTag { get; set; }
	}
}