using Moonlet.Templates.EntityTemplates;
using UnityEngine;

namespace Moonlet.TemplateLoaders.EntityLoaders
{
	public class DecorPlantLoader(DecorPlantTemplate template, string sourceMod) : BasePlantLoader<DecorPlantTemplate>(template, sourceMod)
	{
		protected override Tag GetSeedTag() => GameTags.DecorSeed;

		public override string GetTranslationKey(string partialKey) => $"STRINGS.CREATURES.SPECIES.{id.ToUpperInvariant()}.{partialKey}";

		protected override GameObject CreatePrefab()
		{
			var prefab = base.CreatePrefab();

			var prickleGrass = prefab.AddOrGet<PrickleGrass>();
			prickleGrass.positive_decor_effect = template.DecorAlive.Get();
			prickleGrass.negative_decor_effect = template.DecorWilted.Get();

			return prefab;
		}
	}
}
