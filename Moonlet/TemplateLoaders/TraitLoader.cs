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
			Log.Debug(relativePath);
			id = $"traits{relativePath}";
			template.Id = id;


			nameKey = $"STRINGS.WORLD.TRAITS.{id.ToUpperInvariant()}.NAME";
			descriptionKey = $"STRINGS.WORLD.TRAITS.{id.ToUpperInvariant()}.DESCRIPTION";

			Log.Debug("set internal id of trait: " + template.Id);

			if (template.ContainsKey("name"))
				Log.Debug(template["name"]);

			if (template.ContainsKey("description"))
				Log.Debug(template["description"]);

			var test = GetWorldTrait();
			if (test == null)
			{
				Log.Debug("conversion failed");
			}
			else
			{
				Log.Debug("converted:");
				Log.Debug(test.description);
			}

			base.Initialize();
		}

		public override void RegisterTranslations()
		{
			/*			AddString(nameKey, template.Name);
						AddString(descriptionKey, template.description);

						template.name = nameKey;
						template.description = descriptionKey;*/
		}

		public WorldTrait GetWorldTrait() => Convert<WorldTrait>();
	}
}
