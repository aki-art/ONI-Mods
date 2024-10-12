using Moonlet.Templates.SubTemplates;
using ProcGen;
using System.Collections.Generic;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class SubworldMixingTemplate : BaseTemplate
	{
		public string Description { get; set; }

		public string Icon { get; set; }

		public List<string> ForbiddenClusterTags { get; set; }

		public WeightedSubworldName Subworld { get; set; }

		public List<string> MixingTags { get; set; }

		public List<TemplateSpawnRuleC> AdditionalWorldTemplateRules { get; set; }

		public SubworldMixingTemplate()
		{
			MixingTags = [];
			AdditionalWorldTemplateRules = [];
			ForbiddenClusterTags = [];
		}
	}
}
