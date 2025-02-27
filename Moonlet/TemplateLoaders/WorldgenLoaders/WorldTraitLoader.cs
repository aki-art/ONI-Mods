using Moonlet.Templates.SubTemplates;
using Moonlet.Templates.WorldGenTemplates;
using Moonlet.Utils;
using ProcGen;
using System.Collections.Generic;

namespace Moonlet.TemplateLoaders.WorldgenLoaders
{
	public class WorldTraitLoader(WorldTraitTemplate template, string sourceMod) : TemplateLoaderBase<WorldTraitTemplate>(template, sourceMod)
	{
		public string nameKey;
		public string descriptionKey;

		public override void Initialize()
		{
			id = GetPathId("traits");
			template.Id = id;

			nameKey = $"STRINGS.WORLD.TRAITS.{id.LinkAppropiateFormat()}.NAME";
			descriptionKey = $"STRINGS.WORLD.TRAITS.{id.LinkAppropiateFormat()}.DESCRIPTION";

			base.Initialize();
		}

		public override void RegisterTranslations()
		{
			AddString(nameKey, template.Name);
			AddString(descriptionKey, template.Description);
		}

		public WorldTrait GetWorldTrait()
		{
			var result = CopyProperties<WorldTrait>();

			List<string> traitTags = [];
			if (result.traitTags != null)
				traitTags.AddRange(result.traitTags);
			traitTags.Add(ModTags.MoonletWorldTrait.name);

			result.name = nameKey;
			result.description = descriptionKey;
			result.filePath = id;
			result.additionalSubworldFiles ??= [];
			result.additionalUnknownCellFilters ??= [];
			result.additionalWorldTemplateRules = ShadowTypeUtil.CopyList<ProcGen.World.TemplateSpawnRules, TemplateSpawnRuleTemplate>(template.AdditionalWorldTemplateRules, Warn) ?? [];
			result.removeWorldTemplateRulesById = [];
			result.globalFeatureMods ??= [];
			result.elementBandModifiers ??= [];
			result.exclusiveWith = [];
			result.exclusiveWithTags ??= [];
			result.forbiddenDLCIds ??= [];
			result.traitTags = traitTags;

			if (result.icon.IsNullOrWhiteSpace())
				result.icon = $"{sourceMod}_{relativePath.Replace("/", "")}";

			return result;
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

			traitsDict[worldTrait.filePath] = worldTrait;
		}

		public void LoadIcons()
		{
			// duplicate the sprite because the game doesnt actually use the icon field, instead it reverse engineers an icon name from the path
			if (template.Icon != null)
			{
				string associatedIcon = id.Substring(id.LastIndexOf("/") + 1);
				if (!Assets.Sprites.ContainsKey(associatedIcon))
					if (Assets.Sprites.TryGetValue(template.Icon, out var sprite))
					{
						Assets.Sprites.Add(associatedIcon, sprite);
					}
			}
		}
	}
}
