extern alias YamlDotNetButNew;
using YamlDotNetButNew.YamlDotNet.Serialization;

namespace Moonlet.Templates.SubTemplates
{
	public class MTemplateZoneType
	{
		public string ZoneType { get; set; }

		[YamlMember(Alias = "location_x", ApplyNamingConventions = false)]
		public IntNumber LocationX { get; set; }

		[YamlMember(Alias = "location_y", ApplyNamingConventions = false)]
		public IntNumber LocationY { get; set; }
	}
}
