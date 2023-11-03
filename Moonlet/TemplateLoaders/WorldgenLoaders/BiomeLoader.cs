using Moonlet.Templates.WorldGenTemplates;
using ProcGen;

namespace Moonlet.TemplateLoaders.WorldgenLoaders
{
	public class BiomeLoader(BiomeTemplate template, string sourceMod) : TemplateLoaderBase<BiomeTemplate>(template, sourceMod)
	{
		public override void Initialize()
		{
			id = $"biomes{relativePath}";
			template.Id = id;
			base.Initialize();
		}

		public void LoadContent()
		{
			var table = template.GetValue();
			foreach (var biome in table)
			{
				var key = id + "/" + biome.Key;
				SettingsCache.biomes.BiomeBackgroundElementBandConfigurations[key] = biome.Value;
			}
		}

		public override void RegisterTranslations()
		{
		}
	}
}
