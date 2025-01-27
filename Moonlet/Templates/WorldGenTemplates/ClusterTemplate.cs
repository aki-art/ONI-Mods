extern alias YamlDotNetButNew;

using Moonlet.Templates.SubTemplates;
using Moonlet.Utils;
using ProcGen;
using System.Collections.Generic;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class ClusterTemplate : BaseTemplate
	{
		public List<WorldPlacementC> WorldPlacements { get; set; }
		public List<SpaceMapPOIPlacement> PoiPlacements { get; set; }
		public string Description { get; set; }
		public string WelcomeMessage { get; set; }
		public string[] RequiredDlcIds { get; set; }
		public string ForbiddenDlcId { get; set; }
		public string Difficulty { get; set; }
		public bool DisableStoryTraits { get; set; }
		public IntNumber FixedCoordinate { get; set; }
		public string ClusterCategory { get; set; }
		public List<string> GuaranteedStarterMinions { get; set; }
		public List<string> ClusterTags { get; set; }
		public IntNumber StartWorldIndex { get; set; }
		[Range(4, 4096)]
		public IntNumber Width { get; set; }
		[Range(4, 4096)]
		public IntNumber Height { get; set; }
		[Range(1, 32)]
		public IntNumber NumRings { get; set; }
		public IntNumber MenuOrder { get; set; }
		public string CoordinatePrefix { get; set; }
		public string Skip { get; set; }
		public List<AdditionalTraitRule> AdditionalStoryTraitRules { get; set; }
	}
}
