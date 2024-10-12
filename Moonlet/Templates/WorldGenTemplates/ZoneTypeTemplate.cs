using Moonlet.Templates.SubTemplates;

namespace Moonlet.Templates.WorldGenTemplates
{
	public class ZoneTypeTemplate : BaseTemplate
	{
		public ColorEntry Color { get; set; }

		public string Border { get; set; }

		public TextureEntry Background { get; set; }

		public int BackgroundIndex { get; set; }
	}
}
