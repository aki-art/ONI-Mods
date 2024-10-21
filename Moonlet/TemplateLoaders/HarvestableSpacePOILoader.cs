using Moonlet.Scripts;
using Moonlet.TemplateLoaders.EntityLoaders;
using Moonlet.Templates;
using Moonlet.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Moonlet.TemplateLoaders
{
	public class HarvestableSpacePOILoader(HarvestableSpacePOITemplate template, string sourceMod) : EntityLoaderBase<HarvestableSpacePOITemplate>(template, sourceMod)
	{
		public override string GetTranslationKey(string partialKey) => $"STRINGS.UI.SPACEDESTINATIONS.HARVESTABLE_POI.{template.Id.ToUpperInvariant()}.{partialKey}";

		public override void RegisterTranslations()
		{
			AddString(GetTranslationKey("NAME"), template.Name);
			AddString(GetTranslationKey("DESC"), template.Description);
		}

		private string GetPOITypeId() => template.Id + "_POIType";

		protected override GameObject CreatePrefab()
		{
			if (!ValidateOrbitObjects(template.OrbitalBehavior))
				template.OrbitalBehavior =
				[
					"iceRock",
					"rocky",
					"frozenOre"
				];

			var elements = new Dictionary<SimHashes, float>();

			if (template.Elements == null || template.Elements.Count == 0)
				Error("No elements defined, if you wanted an artifact only POI make an space_destinations/spaced_out/artifact type space POI");

			foreach (var elementEntry in template.Elements)
			{
				if (!ElementUtil.TryGetSimhashIfLoaded(elementEntry.Element, out var element))
				{
					if (!elementEntry.Optional)
						Warn($"Missing element: {elementEntry.Element}");

					continue;
				}

				if (elements.ContainsKey(element))
					Warn($"Duplicate entry of element {elementEntry.Element}");

				elements[element] = elementEntry.Weight.CalculateOrDefault(1);
			}

			var poiType = new HarvestablePOIConfigurator.HarvestablePOIType(
				GetPOITypeId(),
				elements,
				template.Capacity.Min,
				template.Capacity.Max,
				template.Recharge.Min.CalculateOrDefault(30_000),
				template.Recharge.Max.CalculateOrDefault(60_000),
				template.CanProvideArtifact,
				template.OrbitalBehavior,
				template.MaxOrbitingObjects,
				DlcManager.EXPANSION1_ID);

			var prefab = EntityTemplates.CreateEntity(template.Id, template.Name);

			prefab.AddOrGet<SaveLoadRoot>();

			var harvestablePoiConfigurator = prefab.AddOrGet<HarvestablePOIConfigurator>();
			harvestablePoiConfigurator.presetType = GetPOITypeId();

			var harvestablePoiClusterGridEntity = prefab.AddOrGet<Tundra_HarvestablePOIClusterGridEntity>();
			harvestablePoiClusterGridEntity.m_name = template.Name;
			harvestablePoiClusterGridEntity.m_Anim = template.Animation.DefaultAnimation.IsNullOrWhiteSpace()
				? "asteroid_field"
				: template.Animation.DefaultAnimation;
			harvestablePoiClusterGridEntity.animFile = template.Animation.GetFile();

			prefab.AddOrGetDef<HarvestablePOIStates.Def>();

			if (poiType.canProvideArtifacts)
			{
				prefab.AddOrGetDef<ArtifactPOIStates.Def>();
				ArtifactPOIConfigurator artifactPoiConfigurator = prefab.AddOrGet<ArtifactPOIConfigurator>();
				artifactPoiConfigurator.presetType = template.ArtifaceProvider;
			}

			var info = prefab.AddOrGet<InfoDescription>();
			info.description = template.Description;

			return prefab;
		}

		private bool ValidateOrbitObjects(List<string> orbitalBehavior)
		{
			foreach (var behaviour in orbitalBehavior)
			{
				if (Db.Get().OrbitalTypeCategories.TryGet(behaviour) == null)
				{
					var msg = $"{orbitalBehavior} is not a defined orbital behavior! Options are:\n";
					foreach (var item in Db.Get().OrbitalTypeCategories.resources)
					{
						msg += "\t - " + item.Id;
					}

					Warn(msg);
					return false;
				}
			}

			return true;
		}
	}
}
