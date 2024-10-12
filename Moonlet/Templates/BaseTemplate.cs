using System.Collections.Generic;

namespace Moonlet.Templates
{
	public abstract class BaseTemplate : ITemplate
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Priority { get; set; }
		public ITemplate.MergeBehavior Command { get; set; }
		public Dictionary<string, string> PriorityPerClusterTag { get; set; }
		public Dictionary<string, object> Conditions { get; set; }
	}
}
