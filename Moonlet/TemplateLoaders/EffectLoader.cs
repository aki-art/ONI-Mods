using FUtility;
using Moonlet.Templates;
using Moonlet.Utils;

namespace Moonlet.TemplateLoaders
{
	public class EffectLoader(EffectTemplate template, string sourceMod) : TemplateLoaderBase<EffectTemplate>(template, sourceMod)
	{
		public override string GetTranslationKey(string partialKey) => $"STRINGS.DUPLICANTS.MODIFIERS.{id.ToUpper()}.{partialKey}";

		public void LoadContent(ModifierSet set)
		{
			var db = Db.Get();
			var attributes = db.Attributes;
			var amounts = db.Amounts;

			var builder = new EffectBuilder(template.Id, template.DurationSeconds, template.IsBad);

			if (template.Modifiers != null)
			{
				foreach (var modifier in template.Modifiers)
				{
					if (attributes.TryGet(modifier.Id) == null && amounts.TryGet(modifier.Id) == null)
					{
						if (template.Optional)
							Log.Debug($"No modifier with ID {modifier.Id}, skipping {template.Id}", sourceMod);
						else
							Log.Warn($"Error when loading effect {template.Id}, {modifier.Id} is not a valid modifier Id.", sourceMod);

						continue;
					}

					builder.Modifier(modifier.Id, modifier.Value.CalculateOrDefault(0), modifier.IsMultiplier);
				}
			}

			if (!template.Icon.IsNullOrWhiteSpace())
				builder.Icon(template.Icon);

			builder.Add(set);

			if (template.Tags != null)
			{
				ModDb.effectTags ??= [];
				ModDb.effectTags[template.Id] = [.. template.Tags.ToTagList()];
			}
		}

		public override void RegisterTranslations()
		{
			AddString(GetTranslationKey("NAME"), template.Name);
			AddString(GetTranslationKey("TOOLTIP"), template.Tooltip);
		}
	}
}
