using Moonlet.TemplateLoaders;

namespace Moonlet.Loaders
{
	public class MTemplatesLoader() : TemplatesLoader<MTemplateLoader>("templates")
	{
		public override void LoadContent()
		{
			base.LoadContent();
			ApplyToActiveTemplates(template => template.LoadContent());
		}
	}
}
