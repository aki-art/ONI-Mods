using HarmonyLib;
using Moonlet.TemplateLoaders.WorldgenLoaders;
using Moonlet.Templates.SubTemplates;
using Moonlet.Templates.WorldGenTemplates;
using Moonlet.Utils;
using ProcGen;
using System.Collections.Generic;
using System.Linq;
using static ProcGen.World;

namespace Moonlet.TemplateLoaders
{
	public class WorldLoader(WorldTemplate template, string source) : TemplateLoaderBase<WorldTemplate>(template, source), IWorldGenValidator
	{
		public string nameKey;
		public string descriptionKey;
		public static HashSet<string> referencedSubWorldsNotLoadedWithMoonlet = [];

		public override string GetTranslationKey(string partialKey)
		{
			return $"STRINGS.WORLDS.{relativePath.LinkAppropiateFormat()}.{partialKey}";
		}

		public override void Initialize()
		{
			id = $"worlds{relativePath}";
			template.Id = id;

			nameKey = GetTranslationKey("NAME");
			descriptionKey = GetTranslationKey("DESCRIPTION");

			if (template.NamePrefixes != null && template.NamePrefixes.Length > 0)
			{
				var prefixesKey = $"NAMEGEN_{template.Id.ToUpperInvariant()}";
				AddString($"STRINGS.NAMEGEN.WORLD.ROOTS.{prefixesKey}", template.NamePrefixes.Join(delimiter: "\n"));
				template.NameTables ??= [];
				template.NameTables = template.NameTables.AddToArray(prefixesKey);
			}

			base.Initialize();
		}

		public ProcGen.World Get()
		{
			var result = CopyProperties<ProcGen.World>();

			result.name = nameKey;
			result.description = descriptionKey;
			result.filePath = id;
			result.worldsize = template.Worldsize.ToVector2I();

			result.subworldFiles ??= [];
			result.unknownCellsAllowedSubworlds ??= [];
			result.globalFeatures ??= [];
			result.seasons ??= [];
			result.fixedTraits ??= [];
			result.worldTraitRules = ShadowTypeUtil.CopyList<TraitRule, WorldTraitRuleC>(template.WorldTraitRules, Warn) ?? [];
			result.worldTraitScale = template.WorldTraitScale.CalculateOrDefault(1);
			result.iconScale = template.IconScale.CalculateOrDefault(1);
			result.isModded = true;
			result.dlcIdFrom = DlcManager.VANILLA_ID;
			result.worldTags ??= [];

			result.worldTemplateRules = ShadowTypeUtil.CopyList<TemplateSpawnRules, TemplateSpawnRuleC>(template.WorldTemplateRules, Issue) ?? [];

			if (template.WorldTraitRules != null)
			{
				foreach (var rule in template.WorldTraitRules)
				{
					if (rule.AlternativeTraits != null)
					{
						foreach (var altTrait in rule.AlternativeTraits)
						{
							Mod.traitSwaps.Add(new Mod.TraitSwapEntry()
							{
								worldId = template.Id,
								originalTrait = altTrait.Key,
								replacementTrait = altTrait.Value,
							});
						}
					}
				}
			}

			if (!result.startSubworldName.IsNullOrWhiteSpace())
			{
				AddToSubworldListIfMissing(result, result.startSubworldName);
				referencedSubWorldsNotLoadedWithMoonlet.Add(result.startSubworldName);
			}

			foreach (var rule in result.unknownCellsAllowedSubworlds)
			{
				foreach (var subWorld in rule.subworldNames)
				{
					AddToSubworldListIfMissing(result, subWorld);
					referencedSubWorldsNotLoadedWithMoonlet.Add(subWorld);
				}
			}

			return result;
		}

		private void AddToSubworldListIfMissing(ProcGen.World result, string subWorld)
		{
			if (!result.subworldFiles.Any(file => file.name == subWorld))
			{
				Debug("\t " + subWorld);
				result.subworldFiles.Add(new WeightedSubworldName(subWorld, 1f));
			}
		}

		public void LoadContent()
		{
			if (template == null)
				return;

			SettingsCache.worlds.worldCache[Get().filePath] = Get();
		}

		public override void RegisterTranslations()
		{
			AddString(nameKey, template.Name);
			AddString(descriptionKey, template.Description);
		}

		public void ValidateWorldGen()
		{
			var x = template.Worldsize.Get("X");
			var y = template.Worldsize.Get("Y");

			if (x <= 4 || y <= 4)
				Warn($"Issue with world {id}: Size is too small ({x}:{y}). A minimum of 4X4 is required, but at least 32X32 is recommended.");

			foreach (var subworld in template.SubworldFiles)
			{
				if (!SettingsCache.subworlds.ContainsKey(subworld.name))
					Warn($"Issue with world {id}: {subworld.name} is not a registered SubWorld.");
			}

			if (!template.StartSubworldName.IsNullOrWhiteSpace() && !SettingsCache.subworlds.ContainsKey(template.StartSubworldName))
				Warn($"Issue with world {id}: {template.StartSubworldName} is not a registered SubWorld.");

			if (template.StartSubworldName.IsNullOrWhiteSpace() != template.StartingBaseTemplate.IsNullOrWhiteSpace())
				Warn($"Issue with world {id}: A StartSubworldName and a StartingBaseTemplate should be either both defined, or not at all.");

			if (template.FixedTraits != null)
			{
				foreach (var trait in template.FixedTraits)
				{
					if (!SettingsCache.worldTraits.ContainsKey(trait))
						Warn($"Issue with world {id}: {trait} is not a registered WorldTrait.");
				}
			}

			if (template.GlobalFeatures != null)
			{
				foreach (var feature in template.GlobalFeatures)
				{
					if (SettingsCache.featureSettings.ContainsKey(feature.Key))
						Warn($"Issue with world {id}: {feature.Key} is not a registered Feature.");
					// TODO: weird math
				}
			}

			if (template.WorldTemplateRules != null)
			{
				foreach (var rule in template.WorldTemplateRules)
				{
					if (rule.Names == null)
					{
						Warn($"Issue with world {id}:Templaterule {rule.RuleId} has no templated defined.");
						continue;
					}

					foreach (var name in rule.Names)
					{
						if (TemplateCache.GetTemplate(name) == null)
							Warn($"Issue with world {id}: {name} is not a registered Template.");
					}
				}
			}

			if (template.UnknownCellsAllowedSubworlds == null)
				Warn($"Issue with world {id}: No UnknownCellsAllowedSubworlds means no subworlds are allowed!");
			else
			{
				foreach (var subworld in template.SubworldFiles)
				{
					if (subworld.minCount > 0)
					{
						var hasAtLeast1Allowed = false;
						foreach (var rule in template.UnknownCellsAllowedSubworlds)
						{
							if (rule.subworldNames.Contains(subworld.name))
							{
								hasAtLeast1Allowed = true;
								break;
							}
						}

						if (!hasAtLeast1Allowed)
							Warn($"Issue with world {id}: {subworld.name} has requested a minCount of {subworld.minCount}, but none were added to UnknownCellsAllowedSubworlds, so this cannot be fulfilled.");
					}
				}
			}
		}
	}
}