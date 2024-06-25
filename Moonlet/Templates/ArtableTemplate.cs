using System.Collections.Generic;
using static Moonlet.Templates.EntityTemplates.EntityTemplate;

namespace Moonlet.Templates
{
	public class ArtableTemplate : ITemplate
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public string BuildingId { get; set; }

		public bool Optional { get; set; }

		public string Description { get; set; }

		public string Priority { get; set; }

		public Dictionary<string, string> PriorityPerCluster { get; set; }

		public AnimationEntry Animation { get; set; }

		public string Quality { get; set; }

		public int BonusDecor { get; set; }
	}
}
