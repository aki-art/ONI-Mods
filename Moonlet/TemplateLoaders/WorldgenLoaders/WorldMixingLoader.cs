using Moonlet.Templates.WorldGenTemplates;
using ProcGen;

namespace Moonlet.TemplateLoaders.WorldgenLoaders
{
	public class WorldMixingLoader(WorldMixingTemplate template, string sourceMod) : TemplateLoaderBase<WorldMixingTemplate>(template, sourceMod)
	{
		public override void RegisterTranslations()
		{
			AddString(GetTranslationKey("NAME"), template.Name);
			AddString(GetTranslationKey("DESCRIPTION"), template.Description);
		}

		public override string GetTranslationKey(string partialKey)
		{
			return $"STRINGS.WORLDMIXINGS.{template.Id.ToUpperInvariant()}.{partialKey}";
		}

		public override void Initialize()
		{
			id = GetPathId("worldMixing");
			template.Id = id;

			base.Initialize();
		}

		public void LoadContent(ref System.Collections.Generic.ISet<string> referencedWorlds)
		{
			if (template == null)
				return;

			ClusterLoader.referencedWorldsNotLoadedWithMoonlet.Add(template.World);
			referencedWorlds.Add(template.World);
			SettingsCache.worldMixingSettings[template.Id] = Get();
		}

		private WorldMixingSettings Get()
		{
			return new WorldMixingSettings()
			{
				name = GetTranslationKey("NAME"),
				description = GetTranslationKey("DESCRIPTION"),
				forbiddenClusterTags = template.ForbiddenClusterTags,
				world = template.World,
				icon = template.Icon,
			};
		}

		public string GetWorldgenPath() => id;
	}
}
