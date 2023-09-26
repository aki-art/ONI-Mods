using ProcGen;
using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class ClusterTemplate : ClusterLayout, ITemplate
	{
		public string Id { get; set; }

		[YamlIgnore] public string Name { get; set; } // base entry has name

		public int Priority { get; set; }

		[YamlIgnore] public Dictionary<string, int> PriorityPerCluster { get; set; }
	}
}
