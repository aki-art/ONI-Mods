using Moonlet.Templates.WorldGenTemplates;
using ProcGen;
using System.Collections.Generic;

namespace Moonlet.TemplateLoaders
{
	public class WorldTraitLoader(WorldTraitTemplate template, string sourceMod) : TemplateLoaderBase<WorldTraitTemplate>(template, sourceMod)
	{
		public string nameKey;
		public string descriptionKey;

		public override void Initialize()
		{
			id = $"traits{relativePath}";
			template.Id = id;

			nameKey = $"STRINGS.WORLD.TRAITS.{id.ToUpperInvariant()}.NAME";
			descriptionKey = $"STRINGS.WORLD.TRAITS.{id.ToUpperInvariant()}.DESCRIPTION";

			var expectedIconName = id.Substring(id.LastIndexOf("/") + 1);
			if (!expectedIconName.StartsWith(sourceMod))
				Warn($"Trait {id} is not namespaced!");

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

			// duplicate the sprite because the game doesnt actually use the icon field, instead it reverse engineers an icon name from the path
			if (template.Icon != null)
			{
				string associatedIcon = worldTrait.filePath.Substring(worldTrait.filePath.LastIndexOf("/") + 1);
				if (!Assets.Sprites.ContainsKey(associatedIcon))
					if (Assets.Sprites.TryGetValue(template.Icon, out var sprite))
					{
						Assets.Sprites.Add(associatedIcon, sprite);
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

	}
}
