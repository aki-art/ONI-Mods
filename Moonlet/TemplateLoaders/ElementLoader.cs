using Moonlet.Templates;
using System.Collections.Generic;

namespace Moonlet.TemplateLoaders
{
	public class ElementLoader(ElementTemplate template) : TemplateLoaderBase<ElementTemplate>(template)
	{
		public void LoadContent(ref List<Substance> list)
		{
			Debug.Log("Loading element:" + template.Id);

			var element = template;

			var color = Util.ColorFromHex(element.Color);
			var uiColor = Util.ColorFromHex(element.UiColor);
			var conduitColor = Util.ColorFromHex(element.ConduitColor);
			var specularColor = element.SpecularColor.IsNullOrWhiteSpace() ? Color.black : Util.ColorFromHex(element.SpecularColor);
			var anim = GetElementAnim(element);

			var info = new ElementInfo(element.Id, anim, element.State, color);

			var specular = !element.SpecularTexture.IsNullOrWhiteSpace();
			var material = GetElementMaterial(element, list);

			Log.Debuglog("Creating substance for " + element.Id);
			Log.Debuglog("\tkey:" + element.GetNameKey());
			if (Strings.TryGet(element.GetNameKey(), out var name))
				Log.Debuglog(name);
			else
				Log.Debuglog("NAME NOT FOUND");

			if (Strings.TryGet(element.GetDescriptionKey(), out var desc))
				Log.Debuglog(desc);
			else
				Log.Debuglog("DESC NOT FOUND");

			newElements.Add(info.CreateSubstance(element.textureFolder, specular, material, uiColor, conduitColor, specularColor, element.NormalMapTexture));

			element.simHash = info.SimHash;
		}

		public override void RegisterTranslations()
		{
			Mod.translationLoader.Add($"STRINGS.ELEMENTS.{template.Id.ToUpperInvariant()}.NAME", template.Name);
			Mod.translationLoader.Add($"STRINGS.ELEMENTS.{template.Id.ToUpperInvariant()}.DESCRIPTION", template.DescriptionText);
		}
	}
}
