using System.Collections.Generic;
using static Moonlet.Elements.ExtendedElementEntry;

namespace Moonlet.Elements
{
	public class ElementDefaultsEntry : IMerge<ElementDefaultsEntry>, IMerge<ExtendedElementEntry>
	{
		public string materialCategory { get; set; }

		public string[] tags { get; set; }

		public List<CustomDataEntry> customData { get; set; }

		public string defaultPriority { get; set; }

		public ExtendedElementEntry.PrioritySetting[] priorityPerCluster { get; set; }

		public string dlcId { get; set; }

		public ElementDefaultsEntry Merge(ElementDefaultsEntry other)
		{
			other.materialCategory ??= materialCategory;
			other.tags ??= tags;
			other.customData ??= customData;
			other.defaultPriority ??= defaultPriority;
			other.priorityPerCluster ??= priorityPerCluster;
			other.dlcId ??= dlcId;

			return other;
		}

		public ExtendedElementEntry Merge(ExtendedElementEntry other)
		{
			other.MaterialCategory ??= materialCategory;
			other.Tags ??= tags;
			other.CustomData ??= customData;
			other.DefaultPriority ??= defaultPriority;
			other.PriorityPerCluster ??= priorityPerCluster;
			other.DlcId ??= dlcId;

			return other;
		}
	}
}
