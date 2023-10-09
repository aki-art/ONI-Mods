using Moonlet.Templates;
using Moonlet.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Moonlet.TemplateLoaders
{
	public class ElementLoader(ElementTemplate template, string sourceMod) : TemplateLoaderBase<ElementTemplate>(template, sourceMod)
	{
		private static Material oreMaterial;
		private static Material refinedMaterial;
		private static Material gemMaterial;

		private const string UNSTABLE = "Unstable";

		public ElementInfo elementInfo;
		private string nameKey;
		private string descriptionKey;

		public void LoadContent(ref List<Substance> substances)
		{
			CreateSubstance(substances);
			ConfigureRottableAtmosphere();
		}

		public void SetExposureValue(Dictionary<SimHashes, float> customExposureRates)
		{
			if (template.EyeIrritationStrength != null)
				customExposureRates[elementInfo.SimHash] = template.EyeIrritationStrength.Calculate();
		}

		private void CreateSubstance(List<Substance> substances)
		{
			oreMaterial ??= substances.Find(e => e.elementID == SimHashes.Cuprite).material;
			refinedMaterial ??= substances.Find(e => e.elementID == SimHashes.Copper).material;
			gemMaterial ??= substances.Find(e => e.elementID == SimHashes.Diamond).material;

			var element = template;

			var color = element.Color;
			var uiColor = element.UiColor;
			var conduitColor = element.ConduitColor;
			var specularColor = element.SpecularColor == null ? Color.black : (Color)element.SpecularColor;
			var anim = GetElementAnim();

			elementInfo = new ElementInfo(element.Id, anim, element.State, (Color)color);

			var specular = !element.SpecularTexture.IsNullOrWhiteSpace();
			var material = GetElementMaterial(substances);

			var path = MoonletMods.Instance.GetAssetsPath(sourceMod, "elements");

			var substance = elementInfo.CreateSubstance(path, specular, material, uiColor, conduitColor, specularColor, element.NormalMapTexture);

			substances.Add(substance);
		}

		public void ConfigureRottableAtmosphere()
		{
			if (template.RotAtmosphereQuality.IsNullOrWhiteSpace())
				return;

			if (Enum.TryParse<Rottable.RotAtmosphereQuality>(template.RotAtmosphereQuality, out var rot))
			{
				if (rot != Rottable.RotAtmosphereQuality.Normal)
					Rottable.AtmosphereModifier[(int)elementInfo.SimHash] = rot;
			}
		}

		public override void Validate()
		{
			base.Validate();

			Log.Debug($"element id {template.Id}");
			nameKey = $"STRINGS.ELEMENTS.{template.Id.ToUpperInvariant()}.NAME";
			descriptionKey = $"STRINGS.ELEMENTS.{template.Id.ToUpperInvariant()}.DESCRIPTION";

			// elements need early access
			Log.Debug("adding strings to element");

			Strings.Add(nameKey, template.Name);
			Strings.Add(descriptionKey, template.DescriptionText);

			if (template.Color == null || !template.Color.hasValue)
			{
				Log.Warn($"{template.Id} has no color defined!");
				template.Color = new ColorEntry(Color.white);
			}

			template.ConduitColor ??= template.Color.value;
			template.UiColor ??= template.Color.value;
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

			AddString(nameKey, template.Name);
			AddString(descriptionKey, template.DescriptionText);
		}

		public bool IsUnstable() => template.Tags != null && template.Tags.Contains(UNSTABLE);

		public global::ElementLoader.ElementEntry ToElementEntry()
		{
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
