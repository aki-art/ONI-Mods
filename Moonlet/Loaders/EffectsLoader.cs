using Moonlet.TemplateLoaders;

namespace Moonlet.Loaders
{
	public class EffectsLoader(string path) : TemplatesLoader<EffectLoader>(path)
	{
		public void LoadContent(ModifierSet modifierSet)
		{
			Log.Debug("EffectsLoader loading effects");

			ApplyToActiveTemplates(template => template.LoadContent(modifierSet));
		}
	}
}
