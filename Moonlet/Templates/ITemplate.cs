using Moonlet.Utils;
using System.Collections.Generic;

namespace Moonlet.Templates
{
	public interface ITemplate
	{
		[Doc("Unique identifier of this entry")]
		public string Id { get; set; }

		[Doc("Display name of this entry in plain english. A STRINGS entry will be generated for this item and appear in the localization file.")]
		public string Name { get; set; }

		[Doc("Generic Priority. Items with higher priority will replace those with lower in case of duplicate entries.")]
		public string Priority { get; set; }

		[Doc("Use `Merge` to update an existing entry, or `Replace` to entirely replace other entries.")]
		public MergeBehavior Command { get; set; }

		[Doc("Same as priority, but applied per Cluster. This allows you to give higher priority to certain items for specific world gen. For example, to load a germ on your own map with greater guarantee.")]
		public Dictionary<string, string> PriorityPerClusterTag { get; set; }

		public Dictionary<string, object> Conditions { get; set; }

		public Dictionary<string, Dictionary<string, object>> ModData { get; set; }

		public enum MergeBehavior
		{
			Merge,
			Replace
		}
	}
}
