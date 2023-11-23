﻿extern alias YamlDotNetButNew;
using ProcGen;
using System.Collections.Generic;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class WorldTraitTemplate : ITemplate
	{
		public string Priority { get; set; }

		public Dictionary<string, string> PriorityPerCluster { get; set; }

		[YamlIgnore] public string Id { get; set; }

		/// <summary>
		/// Do not appear on worlds unless specifically enabled by tags
		/// </summary>
		//public bool DisableByDefault { get; set; }

		#region original fields

		public string Name { get; set; }

		public string Description { get; set; }

		public string ColorHex { get; set; }

		public string Icon { get; set; }

		public List<string> ForbiddenDLCIds { get; set; }

		public List<string> ExclusiveWith { get; set; }

		public List<string> ExclusiveWithTags { get; set; }

		public List<string> TraitTags { get; set; }

		public MinMax StartingBasePositionHorizontalMod { get; set; }

		public MinMax StartingBasePositionVerticalMod { get; set; }

		public List<WeightedSubworldName> AdditionalSubworldFiles { get; set; }

		public List<ProcGen.World.AllowedCellsFilter> AdditionalUnknownCellFilters { get; set; }

		public List<ProcGen.World.TemplateSpawnRules> AdditionalWorldTemplateRules { get; set; }

		public Dictionary<string, int> GlobalFeatureMods { get; set; }

		public List<string> RemoveWorldTemplateRulesById { get; set; }

		public List<WorldTrait.ElementBandModifier> ElementBandModifiers { get; set; }

		#endregion
	}
}