using Moonlet.Templates;

namespace Moonlet.TemplateLoaders
{
	public class TagLoader(TagTemplate template, string sourceMod) : TemplateLoaderBase<TagTemplate>(template, sourceMod)
	{
		public override string GetTranslationKey(string partialKey)
		{
			return partialKey == "DESC"
				? $"STRINGS.MISC.TAGS.{id.ToUpperInvariant()}_DESC"
				: $"STRINGS.MISC.TAGS.{id.ToUpperInvariant()}";
		}

		public override void RegisterTranslations()
		{
			AddString(GetTranslationKey("NAME"), template.Name);
			AddString(GetTranslationKey("DESC"), template.Description);
		}

		public override void Initialize()
		{
			base.Initialize();
			var tag = TagManager.Create(template.Id);

			if (template.Hidden)
			{
				ModTags.hiddenTags.Add(tag);
			}
		}
	}
}
