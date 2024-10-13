using Moonlet.Templates.WorldGenTemplates;
using ProcGen;

namespace Moonlet.TemplateLoaders.WorldgenLoaders
{
	public class BorderLoader(BorderTemplate template, string sourceMod) : TemplateLoaderBase<BorderTemplate>(template, sourceMod)
	{
		public override void Initialize()
		{
			id = $"{sourceMod}/borders";
			template.Id = id;
			base.Initialize();
		}

		public void LoadEnumReplacement()
		{

		}

		public void LoadContent()
		{
			foreach (var border in template.Add)
				SettingsCache.borders[border.Key] = border.Value;
		}

		public override void RegisterTranslations()
		{
		}
	}
}
