extern alias YamlDotNetButNew;

using Moonlet.Templates.SubTemplates;
using ProcGen;
using System.Collections.Generic;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class WorldTemplate : ITemplate
	{
		public string Id { get; set; }
		public ITemplate.MergeBehavior Command { get; set; }
		public string Name { get; set; }
		public string Priority { get; set; }
		[YamlIgnore] public Dictionary<string, string> PriorityPerCluster { get; set; }
		public string Description { get; set; }
		public string[] NameTables { get; set; }
		public string AsteroidIcon { get; set; }
		public FloatNumber IconScale { get; set; }
		public bool DisableWorldTraits { get; set; }
		public List<WorldTraitRuleC> WorldTraitRules { get; set; }
		public FloatNumber WorldTraitScale { get; set; }
		public List<string> WorldTags { get; set; }
		public bool ModuleInterior { get; set; }
		public ProcGen.World.WorldCategory Category { get; set; }
		public Vector2IC Worldsize { get; set; }
		public DefaultSettings DefaultsOverrides { get; set; }
		public ProcGen.World.LayoutMethod LayoutMethod { get; set; }
		public List<WeightedSubworldName> SubworldFiles { get; set; }
		public List<ProcGen.World.AllowedCellsFilter> UnknownCellsAllowedSubworlds { get; set; }
		public string StartSubworldName { get; set; }
		public string StartingBaseTemplate { get; set; }
		public MinMax StartingBasePositionHorizontal { get; set; } = new MinMax(0.5f, 0.5f);
		public MinMax StartingBasePositionVertical { get; set; } = new MinMax(0.5f, 0.5f);
		public Dictionary<string, int> GlobalFeatures { get; set; }
		public List<TemplateSpawnRuleC> WorldTemplateRules { get; set; }
		public List<string> Seasons { get; set; }
		public List<string> FixedTraits { get; set; }
		public bool AdjacentTemporalTear { get; set; }
		public string[] NamePrefixes { get; set; }
	}
}
