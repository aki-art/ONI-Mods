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
			result.additionalSubworldFiles ??= new List<WeightedSubworldName>();
			result.additionalUnknownCellFilters ??= new List<ProcGen.World.AllowedCellsFilter>();
			result.additionalWorldTemplateRules ??= new List<ProcGen.World.TemplateSpawnRules>();
			result.removeWorldTemplateRulesById = new List<string>();
			result.globalFeatureMods ??= new Dictionary<string, int>();
			result.elementBandModifiers ??= new List<WorldTrait.ElementBandModifier>();
			result.exclusiveWith = new List<string>();
			result.exclusiveWithTags ??= new List<string>();
			result.forbiddenDLCIds ??= new List<string>();
			result.traitTags ??= new List<string>();
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
					if (DlcManager.IsContentEnabled(forbiddenDlcId))
						return;
				}
			}

			foreach (var feature in worldTrait.globalFeatureMods)
			{
				if (!SettingsCache.featureSettings.ContainsKey(feature.Key))
					Log.Error($"Cannot load trait {id}: there is no feature with ID {feature.Key}");
			}

			Log.Debug("loaded trait: " + worldTrait.filePath);
			traitsDict[worldTrait.filePath] = worldTrait;
		}
	}
}
