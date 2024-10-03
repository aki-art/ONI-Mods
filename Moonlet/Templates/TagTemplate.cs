using System.Collections.Generic;

namespace Moonlet.Templates
{
	public class TagTemplate : ITemplate
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool Hidden { get; set; }

		public string Priority { get; set; }
		public ITemplate.MergeBehavior Command { get; set; }
		public Dictionary<string, string> PriorityPerClusterTag { get; set; }
	}
}
