using Moonlet.Utils;
using System;
using System.Collections.Generic;

namespace Moonlet.Templates.SubTemplates
{
	public class WorldTraitRuleC : IShadowTypeBase<ProcGen.World.TraitRule>
	{
		public IntNumber Min { get; set; }
		public IntNumber Max { get; set; }
		public List<string> RequiredTags { get; set; }
		public List<string> SpecificTraits { get; set; }
		public List<string> ForbiddenTags { get; set; }
		public List<string> ForbiddenTraits { get; set; }
		public Dictionary<string, string> AlternativeTraits { get; set; }

		public ProcGen.World.TraitRule Convert(Action<string> log = null)
		{
			return new ProcGen.World.TraitRule()
			{
				min = Min.CalculateOrDefault(0),
				max = Max.CalculateOrDefault(0),
				requiredTags = RequiredTags,
				specificTraits = SpecificTraits,
				forbiddenTags = ForbiddenTags,
				forbiddenTraits = ForbiddenTraits
			};
		}
	}
}
