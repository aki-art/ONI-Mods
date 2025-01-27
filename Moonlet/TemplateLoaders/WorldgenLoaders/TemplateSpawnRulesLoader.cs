using Moonlet.Templates.SubTemplates;
using ProcGen;

namespace Moonlet.TemplateLoaders.WorldgenLoaders
{
	public class TemplateSpawnRulesLoader(TemplateSpawnRuleTemplate template, string sourceMod) : TemplateLoaderBase<TemplateSpawnRuleTemplate>(template, sourceMod)
	{
		public override void RegisterTranslations() { }

		public void ApplyToTrait(WorldTrait trait)
		{
			trait.additionalWorldTemplateRules.Insert(0, template.Convert(Issue));
		}
	}
}
