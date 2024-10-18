using HarmonyLib;
using Moonlet.Templates.EntityTemplates;
using Moonlet.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Moonlet.TemplateLoaders.EntityLoaders
{
	public abstract class BasePlantLoader<PlantTemplateType>(PlantTemplateType template, string sourceMod) : EntityLoaderBase<PlantTemplateType>(template, sourceMod) where PlantTemplateType : BasePlantTemplate
	{
		public override string GetTranslationKey(string partialKey) => $"STRINGS.CREATURES.SPECIES.{id.ToUpperInvariant()}.{partialKey}";

		protected abstract Tag GetSeedTag();

		protected override GameObject CreatePrefab()
		{
			var prefab = EntityTemplates.CreatePlacedEntity(
				id,
				template.Name,
				template.Description,
				template.Mass,
				Assets.GetAnim(template.Animation.GetFile()),
				template.Animation.DefaultAnimation,
				Grid.SceneLayer.Creatures,
				(int)template.Width,
				(int)template.Height,
				template.DecorAlive.Get(),
				default,
				SimHashes.Creature,
				template.Tags?.ToTagList(),
				template.DefaultTemperature ?? 273.15f);

			EntityTemplates.ExtendEntityToBasicPlant(
				prefab,

				template.TemperatureLethalLow,
				template.TemperatureWarningLow,
				template.TemperatureWarningHigh,
				template.TemperatureLethalHigh,

				template.SafeElements?.Select(e => ElementUtil.GetSimhashSafe(e)).ToArray(),

				template.PressureSensitive,
				template.PressureLethalLow,
				template.PressureWarningLow,

				template.CropId,

				template.CanDrown,
				template.CanTinker,
				template.RequiresSolidTile,
				template.ShouldGrowOld,
				template.MaxAge,
				template.RadiationLethalLow,
				template.RadiationLethalHigh,
				$"{id}Original",
				template.Name);

			if (template.Seed != null && !template.Seed.Id.IsNullOrWhiteSpace())
			{
				if (Mod.seedsLoader.TryGet(template.Seed.Id, out var seed))
				{
					var seedTag = GetSeedTag();

					var additionalTags = new List<Tag>()
					{
						seedTag
					};

					if (seed.template.Tags != null)
						additionalTags.AddRange(seed.template.Tags.ToTagList());

					var direction = SingleEntityReceptacle.ReceptacleDirection.Top;
					if (Enum.TryParse<SingleEntityReceptacle.ReceptacleDirection>(template.PlanterDirection, out var dir))
					{
						direction = dir;
					}
					else
					{
						Debug($"Error in {template.Id}, {template.PlanterDirection} is not a valid option. " +
							$"Options: {Enum.GetNames(typeof(SingleEntityReceptacle.ReceptacleDirection)).Join()}");
					}

					var seedPrefab = EntityTemplates.CreateAndRegisterSeedForPlant(
						prefab,
						SeedProducer.ProductionType.Hidden,
						seed.template.Id,
						seed.template.Name,
						seed.template.Description,
						Assets.TryGetAnim(seed.template.Animation.GetFile(), out var anim) ? anim : Assets.GetAnim("seed_potted_cylindricafan_kanim"),
						seed.template.Animation.DefaultAnimation ?? "object",
						template.Seed.Count,
							additionalTags,
						direction,
						seed.template.ReplantGroundTag ?? default,
						seed.template.SortOrder,
						template.Description, // TODO
						EntityTemplates.CollisionShape.CIRCLE,
						seed.template.Width,
						seed.template.Height,
						ignoreDefaultSeedTag: !(seedTag == GameTags.DecorSeed || seedTag == GameTags.CropSeed));


					EntityTemplates.CreateAndRegisterPreviewForPlant(
						seedPrefab,
						$"{template.Id}_preview",
						Assets.GetAnim(template.Animation.GetFile()),
						"place",
						(int)template.Width,
						(int)template.Height);
				}
				else
				{
					Warn($"Seed {template.Seed.Id} was defined, but no seed has been configured under this id in entities/plants/seeds.");
				}
			}

			return prefab;
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
