using Moonlet.Templates.WorldGenTemplates;

namespace Moonlet.TemplateLoaders
{
	public class ClusterLoader(ClusterTemplate template, string source) : TemplateLoaderBase<ClusterTemplate>(template, source)
	{
		public override string GetTranslationKey(string partialKey) => $"STRINGS.WORLDS.{id.ToUpper()}.{partialKey}";

		public void LoadContent()
		{
			Log.Debug("Loading cluster " + template.Id);
			//Log.Debug("whats with name? " + template.name);
		}

		public override void RegisterTranslations()
		{
			AddString(GetTranslationKey("NAME"), template.Name);
			//AddString(GetTranslationKey("DESCRIPTION"), template.description);
		}
	}
}
