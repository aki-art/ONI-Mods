using Moonlet.Templates.SubTemplates;
using Moonlet.Templates.WorldGenTemplates;
using Moonlet.Utils;
using PeterHan.PLib.Core;
using ProcGen;

namespace Moonlet.TemplateLoaders.WorldgenLoaders
{
	public class SubworldMixingLoader(SubworldMixingTemplate template, string sourceMod) : TemplateLoaderBase<SubworldMixingTemplate>(template, sourceMod)
	{
		public override void RegisterTranslations()
		{
			AddString(GetTranslationKey("NAME"), template.Name);
			AddString(GetTranslationKey("DESCRIPTION"), template.Description);
		}

		public override string GetTranslationKey(string partialKey)
		{
			return $"STRINGS.SUBWORLDMIXINGS.{template.Id.ToUpperInvariant()}.{partialKey}";
		}

		public override void Initialize()
		{
			id = $"subworldMixing{relativePath}";
			template.Id = id;

			base.Initialize();
		}

		public void LoadContent()
		{
			if (template == null)
				return;

			WorldLoader.referencedSubWorldsNotLoadedWithMoonlet.Add(template.Subworld.name);

			SettingsCache.subworldMixingSettings[template.Id] = Get();
		}

		private SubworldMixingSettings Get()
		{
			var result = new SubworldMixingSettings()
			{
				name = GetTranslationKey("NAME"),
				description = GetTranslationKey("DESCRIPTION"),
				mixingTags = template.MixingTags,
				additionalWorldTemplateRules = ShadowTypeUtil.CopyList<ProcGen.World.TemplateSpawnRules, TemplateSpawnRuleC>(template.AdditionalWorldTemplateRules, Warn),
				forbiddenClusterTags = template.ForbiddenClusterTags,
				subworld = template.Subworld,
				icon = template.Icon,
			};

			foreach (var rule in template.AdditionalWorldTemplateRules)
			{
				Log.Debug($"{rule.Names.Join()}, {rule.AllowedCellsFilter?[0].ZoneTypes?.Join()}");
			}
			return result;
		}

		internal string GetWorldgenPath()
		{
			return id;
		}
	}
}
