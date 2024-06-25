using Moonlet.Templates.EntityTemplates;
using UnityEngine;

namespace Moonlet.TemplateLoaders.EntityLoaders
{
	public class GenericEntityLoader(EntityTemplate template, string source) : EntityLoaderBase<EntityTemplate>(template, source)
	{
		public override void RegisterTranslations()
		{
			AddString(GetTranslationKey("NAME"), template.Name);
			AddString(GetTranslationKey("DESCRIPTION"), template.Description);
		}

		public override string GetTranslationKey(string partialKey) => $"STRINGS.ENTITIES.MISC.{id.ToUpperInvariant()}.{partialKey}";

		protected override GameObject CreatePrefab() => CreateBasicPlacedEntity();
	}
}
