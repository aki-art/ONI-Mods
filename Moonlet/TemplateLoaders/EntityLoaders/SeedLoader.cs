using Moonlet.Templates.EntityTemplates;
using UnityEngine;

namespace Moonlet.TemplateLoaders.EntityLoaders
{
	public class SeedLoader(SeedTemplate template, string sourceMod) : EntityLoaderBase<SeedTemplate>(template, sourceMod)
	{
		public override string GetTranslationKey(string partialKey) => $"STRINGS.CREATURES.SPECIES.SEEDS.{template.Id.ToUpperInvariant()}.{partialKey}";

		public override void RegisterTranslations()
		{
			AddString(GetTranslationKey("NAME"), template.Name);
			AddString(GetTranslationKey("DESC"), template.Description);
		}

		protected override GameObject CreatePrefab()
		{
			return null;
		}
	}
}
