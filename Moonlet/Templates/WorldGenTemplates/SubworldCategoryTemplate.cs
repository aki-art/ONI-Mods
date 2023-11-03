using System.Collections.Generic;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class SubworldCategoryTemplate : ITemplate
	{
		public string Id { get; set; }
		public string Description { get; set; }
		public string UtilityDescription { get; set; }
		public string Icon { get; set; }
		public string Name { get; set; }
		public string Priority { get; set; }
		public Dictionary<string, string> PriorityPerCluster { get; set; }
	}
}
