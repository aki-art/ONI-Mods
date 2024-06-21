using Moonlet.Templates.EntityTemplates;
using Moonlet.Utils;
using System.Linq;

namespace Moonlet.TemplateLoaders.EntityLoaders
{
	public class DecorPlantLoader(DecorPlantTemplate template, string sourceMod) : EntityLoaderBase<DecorPlantTemplate>(template, sourceMod)
	{
		public override string GetTranslationKey(string partialKey) => $"STRINGS.CREATURES.SPECIES.{id.ToUpperInvariant()}.{partialKey}";

		public override void LoadContent()
		{
			Debug("Loading flower");
			var prefab = EntityTemplates.CreatePlacedEntity(
				id,
				template.Name,
				template.Description,
				template.Mass,
				Assets.GetAnim(template.Animation.File),
				template.Animation.DefaultAnimation,
				Grid.SceneLayer.Creatures,
				(int)template.Width,
				(int)template.Height,
				template.DecorAlive.Get(),
				default,
				SimHashes.Creature,
				template.Tags?.ToTagList(),
				template.DefaultTemperature ?? 273.15f);

			Debug("1");
			EntityTemplates.ExtendEntityToBasicPlant(
				prefab,

				template.TemperatureLethalLow,
				template.TemperatureWarningLow,
				template.TemperatureWarningHigh,
				template.TemperatureLethalHigh,

				template.SafeElements.Select(e => ElementUtil.GetSimhashSafe(e)).ToArray(),

				template.PressureSensitive,
				template.PressureLethalLow,
				template.PressureWarningLow,

				template.CropId,

				can_drown: template.CanDrown,
				false,
				require_solid_tile: template.RequiresSolidTile,
				should_grow_old: true,
				template.MaxAge,
				template.RadiationLethalLow,
				template.RadiationLethalHigh,
				$"{id}Original",
				template.Name);

			Debug("2");
			var seedPrefab = Assets.TryGetPrefab(template.SeedId);

			Debug("3");
			var prickleGrass = prefab.AddOrGet<PrickleGrass>();
			prickleGrass.positive_decor_effect = template.DecorAlive.Get();
			prickleGrass.negative_decor_effect = template.DecorWilted.Get();

			Debug("4");

			if (seedPrefab != null)
			{
				/*				var seed = EntityTemplates.CreateAndRegisterSeedForPlant(
									prefab,
									SeedProducer.ProductionType.DigOnly,
									seedPrefab.PrefabID(),
									seedPrefab.GetProperName(),

								EntityTemplates.CreateAndRegisterPreviewForPlant()

									)*/
			}

			Assets.AddPrefab(prefab.GetComponent<KPrefabID>());
		}

		public override void RegisterTranslations()
		{
			AddString(GetTranslationKey("NAME"), template.Name);
			AddString(GetTranslationKey("DESCRIPTION"), template.Description);
		}

		public override void Initialize()
		{
			id = template.Id;
		}
	}
}
