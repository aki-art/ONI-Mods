using Moonlet.Templates.SubTemplates;
using System.Collections.Generic;

namespace Moonlet.Templates
{
	public class TemplateTemplate : BaseTemplate
	{
		public TemplateInfoC Info { get; set; }
		public List<MTemplateCell> Cells { get; set; }
		public List<MTemplatePrefab> Buildings { get; set; }
		public List<MTemplatePrefab> Pickupables { get; set; }
		public List<MTemplatePrefab> ElementalOres { get; set; }
		public List<MTemplatePrefab> OtherEntities { get; set; }
		public List<MTemplateZoneType> ZoneTypes { get; set; }

		public string UniformZoneType { get; set; }
	}
}
