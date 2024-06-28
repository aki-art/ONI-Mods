extern alias YamlDotNetButNew;

using Moonlet.Utils;
using ProcGen;
using System.Collections.Generic;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class ClusterTemplate : ITemplate
	{
		public string Priority { get; set; }
		[YamlIgnore] public Dictionary<string, string> PriorityPerCluster { get; set; }
		public string Id { get; set; }
		public string Name { get; set; }// base entry has name
		public List<WorldPlacement> WorldPlacements { get; set; }
		public List<SpaceMapPOIPlacement> PoiPlacements { get; set; }
		public string Description { get; set; }
		public string[] RequiredDlcIds { get; set; }
		public string ForbiddenDlcId { get; set; }
		public string Difficulty { get; set; }
		public bool DisableStoryTraits { get; set; }
		public IntNumber FixedCoordinate { get; set; }
		public string ClusterCategory { get; set; }
		public IntNumber StartWorldIndex { get; set; }
		[Range(4, 4096)]
		public IntNumber Width { get; set; }
		[Range(4, 4096)]
		public IntNumber Height { get; set; }
		[Range(1, 32)]
		public IntNumber NumRings { get; set; }
		public IntNumber MenuOrder { get; set; }
		public string CoordinatePrefix { get; set; }
	}
}
