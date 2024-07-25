using System.Collections.Generic;

namespace Moonlet.Templates
{
	public interface ITemplate
	{
		// Unique identifier of this item
		public string Id { get; set; }

		public string Name { get; set; }

		// Generic Priority. Items with higher priority will replace those with lower in case of duplicate entries.
		public string Priority { get; set; }

		// Same as priority, but applied per Cluster. This allows you to give higher priority to certain items for specific world gen. For example, to load a germ on your own map with greater guarantee.
		public Dictionary<string, string> PriorityPerCluster { get; set; }
	}
}
