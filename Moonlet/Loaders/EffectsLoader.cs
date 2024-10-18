using Moonlet.TemplateLoaders;

namespace Moonlet.Loaders
{
	public class EffectsLoader(string path) : TemplatesLoader<EffectLoader>(path)
	{
		public void LoadContent(ModifierSet modifierSet)
		{
			ApplyToActiveLoaders(template => template.LoadContent(modifierSet));
		}
	}
}
