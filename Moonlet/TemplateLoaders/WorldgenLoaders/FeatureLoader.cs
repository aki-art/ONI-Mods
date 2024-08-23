using Moonlet.Templates.WorldGenTemplates;
using ProcGen;
using System.Collections.Generic;
using System.Linq;

namespace Moonlet.TemplateLoaders.WorldgenLoaders
{
	public class FeatureLoader(FeatureTemplate template, string sourceMod) : TemplateLoaderBase<FeatureTemplate>(template, sourceMod), IWorldGenValidator
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

			if (template.ElementChoiceGroups == null)
				result.ElementChoiceGroups = template.ElementChoiceGroupsUppercase;

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

		public void LoadContent()
		{
			if (SettingsCache.featureSettings.TryGetValue(id, out var feature) && feature != null)
				return;

			feature = GetFeature();

			if (feature == null)
				return;

			SettingsCache.featureSettings[id] = feature;
			// TODO : biome
		}

		public override void RegisterTranslations()
		{
		}

		public void ValidateWorldGen()
		{
			if (feature.ElementChoiceGroups == null)
			{
				Issue("No ElementCgoiceGroups defined");
			}
			else
			{
				foreach (var item in feature.ElementChoiceGroups)
				{
					if (item.Value.choices == null || item.Value.choices.Count == 0)
					{
						Issue("No elements defined in " + item.Key);
					}
					else
						foreach (var element in item.Value.choices)
						{
							if (global::ElementLoader.GetElement(element.element) == null)
								Issue($"{element.element} is not a registered Element.");
						}
				}
			}
		}
	}
}
