using Moonlet.Loaders;
using Moonlet.Templates;

namespace Moonlet.TemplateLoaders
{
	public class EffectLoader(EffectTemplate template) : TemplateLoaderBase<EffectTemplate>(template)
	{
		public override string GetTranslationKey(string partialKey) => $"STRINGS.DUPLICANTS.MODIFIERS.{id.ToUpper()}.{partialKey}";

		public void LoadContent(ModifierSet set)
		{
			Log.Debug("Loading effect " + template.Id);
		}

		public override void RegisterTranslations()
		{
			Mod.translationLoader.Add(GetTranslationKey("NAME"), template.Name);
			Mod.translationLoader.Add(GetTranslationKey("TOOLTIP"), template.Tooltip);
		}
	}
}
