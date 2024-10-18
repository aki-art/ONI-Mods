using Moonlet.TemplateLoaders;

namespace Moonlet.Loaders
{
	public class MTemplatesLoader() : TemplatesLoader<MTemplateLoader>("templates")
	{
		public void CacheLoaders()
		{
			base.LoadContent();
			ApplyToActiveLoaders(template => template.GetOrLoad());
		}
	}
}
