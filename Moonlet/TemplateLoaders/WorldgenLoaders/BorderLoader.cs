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
			Log.Debug("loading borders");

			Log.Debug($"LOADED BORDERS {SettingsCache.borders.Count}");
			foreach (var border in template.Add)
			{
				Log.Debug($"{border.Key} {border.Value?.Count}");
				SettingsCache.borders[border.Key] = border.Value;
			}
		}

		public override void RegisterTranslations()
		{
		}
	}
}
