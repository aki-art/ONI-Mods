using Moonlet.Templates.WorldGenTemplates;
using ProcGen;
using System.Collections.Generic;
using System.Linq;

namespace Moonlet.TemplateLoaders.WorldgenLoaders
{
	public class FeatureLoader(FeatureTemplate template, string sourceMod) : TemplateLoaderBase<FeatureTemplate>(template, sourceMod)
	{
		private FeatureSettings feature;

		public override void Initialize()
		{
			Log.Debug(relativePath);
			id = $"features{relativePath}";
			template.Id = id;
			base.Initialize();
		}

		public FeatureSettings GetFeature()
		{
			var result = CopyProperties<FeatureSettings>();

			result.ElementChoiceGroups ??= new Dictionary<string, ElementChoiceGroup<WeightedSimHash>>();
			result.borders ??= new List<int>();
			result.tags ??= new List<string>();
			result.internalMobs ??= new List<MobReference>();

			if (result.borders == null && result.ElementChoiceGroups.Count > 1)
			{
				Warn($"Feature {id} has no border sizes defined.");
				result.borders = Enumerable.Repeat(1, result.ElementChoiceGroups.Count - 1).ToList();
			}

			return result;
		}

		public void LoadContent(Dictionary<string, FeatureSettings> features)
		{
			if (template == null || features.ContainsKey(id))
				return;

			feature = GetFeature();

			if (feature == null)
				return;

			features[id] = feature;
			// TODO : biome
		}

		public override void RegisterTranslations()
		{
		}
	}
}
