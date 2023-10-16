using Moonlet.Templates.WorldGenTemplates;
using Moonlet.Utils;
using ProcGen;
using System.Collections.Generic;
using System.Linq;

namespace Moonlet.TemplateLoaders
{
	public class WorldLoader(WorldTemplate template, string source) : TemplateLoaderBase<WorldTemplate>(template, source)
	{
		public string nameKey;
		public string descriptionKey;

		public override void Initialize()
		{
			id = $"worlds{relativePath}";
			template.Id = id;

			nameKey = $"STRINGS.WORLDS.{relativePath.LinkAppropiateFormat()}.NAME";
			descriptionKey = $"STRINGS.WORLDS.{relativePath.LinkAppropiateFormat()}.DESCRIPTION";

			base.Initialize();
		}

		public override void Validate()
		{
			base.Validate();
		}

		public ProcGen.World Get()
		{
			var result = CopyProperties<ProcGen.World>(log: true);

			result.name = nameKey;
			result.description = descriptionKey;
			result.filePath = id;
			result.worldsize = template.Worldsize.ToVector2I();

			result.subworldFiles ??= new List<WeightedSubworldName>();
			result.unknownCellsAllowedSubworlds ??= new List<ProcGen.World.AllowedCellsFilter>();
			result.globalFeatures ??= new Dictionary<string, int>();
			result.seasons ??= new List<string>();
			result.fixedTraits ??= new List<string>();
			result.worldTraitScale = template.WorldTraitScale.CalculateOrDefault(1);
			result.iconScale = template.IconScale.CalculateOrDefault(1);
			result.worldTraitRules ??= new List<ProcGen.World.TraitRule>();

			result.worldTemplateRules = template.WorldTraitRules != null
				? template.WorldTemplateRules.Select(t => t.Convert()).ToList()
				: new();

			foreach (var rule in result.unknownCellsAllowedSubworlds)
			{
				foreach (var subWorld in rule.subworldNames)
				{
					if (!result.subworldFiles.Any(file => file.name == subWorld))
						result.subworldFiles.Add(new WeightedSubworldName(subWorld, 1f));
				}
			}

			return result;
		}

		public void LoadContent(Dictionary<string, ProcGen.World> worlds)
		{
			Debug("--------------------------------------------");
			Debug("Loading world: " + id);
			Debug($"icon: {template.AsteroidIcon}");
			Debug($"worldsize 1: {template.Worldsize.ToVector2I()}");

			if (template == null)
				return;

			var world = Get();

			worlds[world.filePath] = world;
		}

		public override void RegisterTranslations()
		{
			AddString(nameKey, template.Name);
			AddString(descriptionKey, template.Description);
		}
	}
}
