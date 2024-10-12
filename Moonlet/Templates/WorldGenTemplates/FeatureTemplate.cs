extern alias YamlDotNetButNew;
using ProcGen;
using System.Collections.Generic;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class FeatureTemplate : BaseTemplate
	{
		public ProcGen.Room.Shape Shape { get; set; }
		public List<int> Borders { get; set; }
		public MinMax BlobSize { get; set; }
		public string ForceBiome { get; set; }
		public List<string> BiomeTags { get; set; }
		public List<MobReference> InternalMobs { get; set; }
		public List<string> Tags { get; set; }
		public Dictionary<string, ElementChoiceGroup<WeightedSimHash>> ElementChoiceGroups { get; set; }
		// Inconsistent casing in original files, this alt is here to accept either version
		[YamlMember(Alias = "ElementChoiceGroups", ApplyNamingConventions = false)]
		public Dictionary<string, ElementChoiceGroup<WeightedSimHash>> ElementChoiceGroupsUppercase { get; set; }
	}
}
