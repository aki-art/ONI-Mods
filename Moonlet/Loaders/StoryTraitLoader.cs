using Moonlet.TemplateLoaders;
using Moonlet.Templates.WorldGenTemplates;
using ProcGen;
using System.Collections.Generic;

namespace Moonlet.Loaders
{
	public class StoryTraitLoader(StoryTraitTemplate template, string sourceMod) : TemplateLoaderBase<StoryTraitTemplate>(template, sourceMod)
	{
		public string nameKey;
		public string descriptionKey;
		public WorldTrait worldTrait;

		public override void Initialize()
		{
			id = $"storytraits{relativePath}";
			template.Id = id;

			nameKey = $"STRINGS.WORLD.STORY_TRAITS.{id.ToUpperInvariant()}.NAME";
			descriptionKey = $"STRINGS.WORLD.STORY_TRAITS.{id.ToUpperInvariant()}.DESCRIPTION";

			base.Initialize();
		}

		public override void RegisterTranslations()
		{
			AddString(nameKey, template.Name);
			AddString(descriptionKey, template.Description);
		}

		public void LoadContent(Dictionary<string, WorldTrait> traitsDict)
		{
			if (template == null)
				return;

			var worldTrait = GetWorldTrait();

			if (worldTrait == null)
				return;

			if (worldTrait.forbiddenDLCIds != null)
			{
				foreach (string forbiddenDlcId in worldTrait.forbiddenDLCIds)
				{
					// TODO: Dlc check
					if (DlcManager.IsContentSubscribed(forbiddenDlcId))
						return;
				}
			}

			if (worldTrait.globalFeatureMods != null)
			{
				foreach (var feature in worldTrait.globalFeatureMods)
				{
					if (!SettingsCache.featureSettings.ContainsKey(feature.Key))
						Log.Error($"Cannot load trait {id}: there is no feature with ID {feature.Key}");
				}
			}

			//traitsDict[worldTrait.filePath] = worldTrait;
			this.worldTrait = worldTrait;
		}

		public WorldTrait GetWorldTrait()
		{
			var result = CopyProperties<WorldTrait>();

			result.name = nameKey;
			result.description = descriptionKey;
			result.filePath = id;
			result.additionalSubworldFiles ??= [];
			result.additionalUnknownCellFilters ??= [];
			result.additionalWorldTemplateRules ??= [];
			result.removeWorldTemplateRulesById = [];
			result.globalFeatureMods ??= [];
			result.elementBandModifiers ??= [];
			result.exclusiveWith = [];
			result.exclusiveWithTags ??= [];
			result.forbiddenDLCIds ??= [];
			result.traitTags ??= [];
			result.icon ??= "";

			return result;
		}

	}
}
