using System.Collections.Generic;

namespace Moonlet.Templates
{
	public interface ITemplate
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public string Priority { get; set; }

		public Dictionary<string, string> PriorityPerCluster { get; set; }
	}
}
