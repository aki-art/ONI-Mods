using Moonlet.Templates.WorldGenTemplates;
using Moonlet.Utils;

namespace Moonlet.TemplateLoaders.WorldgenLoaders
{
	public class SubworldCategoryLoader(SubworldCategoryTemplate template, string source) : TemplateLoaderBase<SubworldCategoryTemplate>(template, source), IWorldGenValidator
	{
		public override void RegisterTranslations()
		{
			AddString($"STRINGS.SUBWORLDS.{id.LinkAppropiateFormat()}.NAME", template.Name);
			AddString($"STRINGS.SUBWORLDS.{id.LinkAppropiateFormat()}.DESC", template.Description);
			AddString($"STRINGS.SUBWORLDS.{id.LinkAppropiateFormat()}.UTILITY", template.UtilityDescription);
		}

		public void ValidateWorldGen()
		{
			if (Assets.GetSprite(template.Icon) == null)
				Warn($"Issue at Subworld Category {id}, icon {template.Icon} does not exist");
		}
	}
}
