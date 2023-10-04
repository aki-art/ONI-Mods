using Moonlet.Templates;
using Moonlet.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Moonlet.TemplateLoaders
{
	public class ElementLoader(ElementTemplate template) : TemplateLoaderBase<ElementTemplate>(template)
	{
		private static Material oreMaterial;
		private static Material refinedMaterial;
		private static Material gemMaterial;

		public ElementInfo elementInfo;
		private string nameKey;
		private string descriptionKey;

		public void LoadContent(ref List<Substance> substances)
		{
			oreMaterial ??= substances.Find(e => e.elementID == SimHashes.Cuprite).material;
			refinedMaterial ??= substances.Find(e => e.elementID == SimHashes.Copper).material;
			gemMaterial ??= substances.Find(e => e.elementID == SimHashes.Diamond).material;

			Debug($"Loading element:{template.Id}");

			var element = template;

			var color = Util.ColorFromHex(element.Color);
			var uiColor = Util.ColorFromHex(element.UiColor);
			var conduitColor = Util.ColorFromHex(element.ConduitColor);
			var specularColor = element.SpecularColor.IsNullOrWhiteSpace() ? Color.black : Util.ColorFromHex(element.SpecularColor);
			var anim = GetElementAnim();

			elementInfo = new ElementInfo(element.Id, anim, element.State, color);

			var specular = !element.SpecularTexture.IsNullOrWhiteSpace();
			var material = GetElementMaterial(substances);

			Debug($"Creating substance for {element.Id}");

			var path = MoonletMods.Instance.GetAssetsPath(sourceMod, "elements");
			Debug($"Assets path: {path}");

			var substance = elementInfo.CreateSubstance(path, specular, material, uiColor, conduitColor, specularColor, element.NormalMapTexture);

			substances.Add(substance);
		}

		public override void Validate()
		{
			base.Validate();

			nameKey = $"STRINGS.ELEMENTS.{template.Id.ToUpperInvariant()}.NAME";
			descriptionKey = $"STRINGS.ELEMENTS.{template.Id.ToUpperInvariant()}.DESCRIPTION";

			// elements need early access
			Strings.Add(nameKey, template.Name);
			Strings.Add(descriptionKey, template.DescriptionText);

			template.ConduitColor ??= template.Color;
			template.UiColor ??= template.Color;
			template.DlcId ??= DlcManager.VANILLA_ID;
		}

		private Material GetElementMaterial(List<Substance> substances)
		{
			if (!template.MaterialReference.IsNullOrWhiteSpace())
			{
				var refElement = substances.Find(e => e.elementID.ToString() == template.MaterialReference);

				if (refElement != null)
					return refElement.material;

				Warn($"{template.Id} has asked to reference the material of {template.MaterialReference}, but there is no such element in the game.");
			}

			if (template.SpecularTexture.IsNullOrWhiteSpace())
				return null;

			if (template.MaterialCategory == GameTags.RefinedMetal.ToString())
				return refinedMaterial;

			if (template.MaterialCategory == GameTags.Metal.ToString())
				return oreMaterial;

			return gemMaterial;
		}

		private string GetElementAnim()
		{
			if (!template.DebrisAnim.IsNullOrWhiteSpace())
				return template.DebrisAnim;

			return template.State switch
			{
				Element.State.Gas => "gas_tank_kanim",
				Element.State.Liquid => "liquid_tank_kanim",
				_ => template.Id.ToLowerInvariant() + "_kanim",
			};
		}

		public override void RegisterTranslations()
		{
			nameKey = $"STRINGS.ELEMENTS.{template.Id.ToUpperInvariant()}.NAME";
			descriptionKey = $"STRINGS.ELEMENTS.{template.Id.ToUpperInvariant()}.DESCRIPTION";

			Mod.translationLoader.Add(nameKey, template.Name);
			Mod.translationLoader.Add(descriptionKey, template.DescriptionText);
		}

		public global::ElementLoader.ElementEntry ToElementEntry()
		{
			Log.Debug("template.Toxicity");
			Log.Debug(template.DefaultTemperature.CalculateOrDefault());
			Log.Debug("x");

			return new global::ElementLoader.ElementEntry()
			{
				elementId = template.Id,
				specificHeatCapacity = template.SpecificHeatCapacity.CalculateOrDefault(),
				thermalConductivity = template.ThermalConductivity.CalculateOrDefault(),
				solidSurfaceAreaMultiplier = template.SolidSurfaceAreaMultiplier.CalculateOrDefault(),
				liquidSurfaceAreaMultiplier = template.LiquidSurfaceAreaMultiplier.CalculateOrDefault(),
				gasSurfaceAreaMultiplier = template.GasSurfaceAreaMultiplier.CalculateOrDefault(),
				defaultMass = template.DefaultMass.CalculateOrDefault(),
				defaultTemperature = template.DefaultTemperature.CalculateOrDefault(),
				defaultPressure = template.DefaultPressure.CalculateOrDefault(),
				molarMass = template.MolarMass.CalculateOrDefault(),
				lightAbsorptionFactor = template.LightAbsorptionFactor.CalculateOrDefault(),
				radiationAbsorptionFactor = template.RadiationAbsorptionFactor.CalculateOrDefault(),
				radiationPer1000Mass = template.RadiationPer1000Mass.CalculateOrDefault(),
				lowTempTransitionTarget = template.LowTempTransitionTarget,
				lowTemp = template.LowTemp.CalculateOrDefault(),
				highTempTransitionTarget = template.HighTempTransitionTarget,
				highTemp = template.HighTemp.CalculateOrDefault(),
				lowTempTransitionOreId = template.LowTempTransitionOreId,
				lowTempTransitionOreMassConversion = template.LowTempTransitionOreMassConversion.CalculateOrDefault(),
				highTempTransitionOreId = template.HighTempTransitionOreId,
				highTempTransitionOreMassConversion = template.HighTempTransitionOreMassConversion.CalculateOrDefault(),
				sublimateId = template.SublimateId,
				sublimateFx = template.SublimateFx,
				sublimateRate = template.SublimateRate.CalculateOrDefault(),
				sublimateEfficiency = template.SublimateEfficiency.CalculateOrDefault(),
				sublimateProbability = template.SublimateProbability.CalculateOrDefault(),
				offGasPercentage = template.OffGasPercentage.CalculateOrDefault(),
				materialCategory = template.MaterialCategory,
				tags = template.Tags,
				isDisabled = template.IsDisabled.GetValueOrDefault(),
				strength = template.Strength.CalculateOrDefault(),
				maxMass = template.MaxMass.CalculateOrDefault(),
				hardness = template.Hardness.GetValueOrDefault(),
				toxicity = template.Toxicity.CalculateOrDefault(),
				liquidCompression = template.LiquidCompression.CalculateOrDefault(),
				speed = template.Speed.CalculateOrDefault(),
				minHorizontalFlow = template.MinHorizontalFlow.CalculateOrDefault(),
				minVerticalFlow = template.MinVerticalFlow.CalculateOrDefault(),
				convertId = template.ConvertId,
				flow = template.Flow.CalculateOrDefault(),
				buildMenuSort = template.BuildMenuSort.GetValueOrDefault(),
				state = template.State,
				localizationID = nameKey,
				dlcId = template.DlcId,
				composition = template.Composition,
				description = descriptionKey,
			};
		}
	}
}
