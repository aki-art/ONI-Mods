using Moonlet.Scripts;
using Moonlet.Templates.EntityTemplates;
using Moonlet.Utils;
using UnityEngine;

namespace Moonlet.TemplateLoaders.EntityLoaders
{
	public class SingleHarvestPlantLoader(SingleHarvestPlantTemplate template, string sourceMod) : EntityLoaderBase<SingleHarvestPlantTemplate>(template, sourceMod)
	{
		public override string GetTranslationKey(string partialKey) => $"STRINGS.CREATURES.SPECIES.{id.ToUpperInvariant()}.{partialKey}";

		public override void RegisterTranslations()
		{
			AddBasicString("NAME", template.Name);
			AddBasicString("DESCRIPTION", template.Description);
		}

		protected override GameObject CreatePrefab()
		{
			var prefab = CreateBasicPlacedEntity();

			prefab.AddOrGet<SimTemperatureTransfer>();
			prefab.AddOrGet<OccupyArea>().objectLayers = [ObjectLayer.Building];
			prefab.AddOrGet<EntombVulnerable>();
			prefab.AddOrGet<Prioritizable>();

			var uprootable = prefab.AddOrGet<Moonlet_ExtendedUprootable>();
			uprootable.deathAnimation = template.DeathAnimation ?? "harvest";

			prefab.AddOrGet<UprootedMonitor>();
			prefab.AddOrGet<Harvestable>();
			prefab.AddOrGet<HarvestDesignatable>();
			prefab.AddOrGet<SeedProducer>().Configure(template.CropId, SeedProducer.ProductionType.DigOnly, 1);

			var singleHarvestable = prefab.AddOrGet<Moonlet_SingleHarvestable>();
			singleHarvestable.deathFx = template.DeathFx;
			singleHarvestable.deathAnimation = template.DeathAnimation;
			singleHarvestable.playMode = EnumUtils.ParseOrDefault(template.Animation?.PlayMode, KAnim.PlayMode.Once);
			singleHarvestable.defaultAnimation = template.Animation.DefaultAnimation ?? "idle";

			prefab.AddOrGet<KBatchedAnimController>().randomiseLoopedOffset = true;

			return prefab;
		}
	}
}
