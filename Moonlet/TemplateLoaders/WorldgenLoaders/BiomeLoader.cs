using Moonlet.Templates.WorldGenTemplates;
using ProcGen;

namespace Moonlet.TemplateLoaders.WorldgenLoaders
{
	public class BiomeLoader(BiomeTemplate template, string sourceMod) : TemplateLoaderBase<BiomeTemplate>(template, sourceMod)
	{
		public override void Initialize()
		{
			id = GetPathId("biomes");
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

				foreach (var gradient in biome.Value)
					Mod.elementsLoader.RequestElement(gradient.content, sourceMod);
			}
		}

		public override void RegisterTranslations()
		{
		}
	}
}
