extern alias YamlDotNetButNew;

using ProcGen;
using System.Collections.Generic;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class BorderTemplate : ITemplate
	{
		public Dictionary<string, List<WeightedSimHash>> Add { get; set; }
		public ITemplate.MergeBehavior Command { get; set; }
		public List<string> Remove { get; set; }
		public string Id { get; set; }
		public string Name { get; set; }
		public string Priority { get; set; }
		[YamlIgnore] public Dictionary<string, string> PriorityPerClusterTag { get; set; }
	}
}
