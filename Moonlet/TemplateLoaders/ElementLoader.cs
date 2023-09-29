using Moonlet.Templates;
using System.Collections.Generic;

namespace Moonlet.TemplateLoaders
{
	public class ElementLoader(ElementTemplate template) : TemplateLoaderBase<ElementTemplate>(template)
	{
		public void LoadContent(ref List<Substance> list)
		{
			Debug.Log("Loading element:" +  template.Id);
		}

		public override void RegisterTranslations()
		{
			Mod.translationLoader.Add($"STRINGS.ELEMENTS.{template.Id.ToUpperInvariant()}.NAME", template.Name);
			Mod.translationLoader.Add($"STRINGS.ELEMENTS.{template.Id.ToUpperInvariant()}.DESCRIPTION", template.DescriptionText);
		}
	}
}
