using Moonlet.Templates.SubTemplates;
using System.Collections.Generic;

namespace Moonlet.Templates
{
	public class TemplateTemplate : ITemplate
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Priority { get; set; }
		public ITemplate.MergeBehavior Command { get; set; }
		public Dictionary<string, string> PriorityPerClusterTag { get; set; }

		public TemplateInfoC Info { get; set; }
		public List<MTemplateCell> Cells { get; set; }
		public List<MTemplatePrefab> Buildings { get; set; }
		public List<MTemplatePrefab> Pickupables { get; set; }
		public List<MTemplatePrefab> ElementalOres { get; set; }
		public List<MTemplatePrefab> OtherEntities { get; set; }
	}
}
