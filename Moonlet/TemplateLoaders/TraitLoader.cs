using Moonlet.Templates.WorldGenTemplates;
using ProcGen;

namespace Moonlet.TemplateLoaders
{
	public class TraitLoader(TraitTemplate template, string sourceMod) : TemplateLoaderBase<TraitTemplate>(template, sourceMod)
	{
		public string nameKey;
		public string descriptionKey;

		public override void Initialize()
		{
			base.Initialize();
			id = System.IO.Path.GetFileName(sourceMod);

			nameKey = $"STRINGS.WORLD.TRAITS.{id.ToUpperInvariant()}.NAME";
			descriptionKey = $"STRINGS.WORLD.TRAITS.{id.ToUpperInvariant()}.DESCRIPTION";
		}

		public override void RegisterTranslations()
		{
			AddString(nameKey, template.Name);
			AddString(descriptionKey, template.description);

			template.name = nameKey;
			template.description = descriptionKey;
		}

		public WorldTrait GetWorldTrait() => template;
	}
}
