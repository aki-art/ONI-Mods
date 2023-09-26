using Moonlet.Templates.WorldGenTemplates;

namespace Moonlet.TemplateLoaders
{
	public class ClusterLoader(ClusterTemplate template) : TemplateLoaderBase<ClusterTemplate>(template)
	{
		public override string GetTranslationKey(string partialKey) => $"STRINGS.WORLDS.{id.ToUpper()}.{partialKey}";

		public void LoadContent()
		{
			Log.Debug("Loading cluster " + template.Id);
			Log.Debug("whats with name? " + template.name);
		}

		public override void RegisterTranslations()
		{
			Mod.translationLoader.Add(GetTranslationKey("NAME"), template.Name);
			Mod.translationLoader.Add(GetTranslationKey("DESCRIPTION"), template.description);
		}
	}
}
