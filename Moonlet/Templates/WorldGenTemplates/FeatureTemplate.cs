extern alias YamlDotNetButNew;
using ProcGen;
using System.Collections.Generic;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class FeatureTemplate : ITemplate
	{
		[YamlIgnore] public string Id { get; set; }
		[YamlIgnore] public string Name { get; set; }
		public string Priority { get; set; }
		[YamlIgnore] public Dictionary<string, string> PriorityPerCluster { get; set; }
		public ProcGen.Room.Shape Shape { get; set; }
		public List<int> Borders { get; set; }
		public MinMax BlobSize { get; set; }
		public string ForceBiome { get; set; }
		public List<string> BiomeTags { get; set; }
		public List<MobReference> InternalMobs { get; set; }
		public List<string> Tags { get; set; }
		public Dictionary<string, ElementChoiceGroup<WeightedSimHash>> ElementChoiceGroups { get; set; }
	}
}
