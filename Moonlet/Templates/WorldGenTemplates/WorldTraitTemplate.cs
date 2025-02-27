extern alias YamlDotNetButNew;

using Moonlet.Templates.SubTemplates;
using Moonlet.Utils;
using ProcGen;
using System.Collections.Generic;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class WorldTraitTemplate : BaseTemplate
	{
		/// <summary>
		/// Do not appear on worlds unless specifically enabled by tags
		/// </summary>
		//public bool DisableByDefault { get; set; }

		#region original fields

		[Doc("Rather than loading a new trait, target this exiting trait to override. Use together with Conditions to alter world traits specific to clusters.")]
		public string OverrideId { get; set; }

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

		public List<TemplateSpawnRuleTemplate> AdditionalWorldTemplateRules { get; set; }

		public Dictionary<string, int> GlobalFeatureMods { get; set; }

		public List<string> RemoveWorldTemplateRulesById { get; set; }

		public List<WorldTrait.ElementBandModifier> ElementBandModifiers { get; set; }

		#endregion
	}
}
