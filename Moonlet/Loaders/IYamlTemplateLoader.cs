using Moonlet.Templates;

namespace Moonlet.Loaders
{
	public interface IYamlTemplateLoader
	{
		public void LoadYamls<TemplateType>(MoonletMod mod, bool singleEntry) where TemplateType : class, ITemplate;

		public void SetPath(string path);
	}
}
