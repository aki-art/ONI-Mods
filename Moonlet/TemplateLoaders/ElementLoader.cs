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

			if (!template.HighTempKelvin.HasValue && template.HighTempCelsius.HasValue)
			{
				Log.Debug("HighTempKelvin to " + template.HighTempCelsius.Value);
				template.HighTempKelvin = GameUtil.GetTemperatureConvertedToKelvin(template.HighTempCelsius.Value, GameUtil.TemperatureUnit.Celsius);
			}
			Log.Debug("HighTempKelvin is " + template.HighTempKelvin.Value);

			if (!template.LowTempKelvin.HasValue && template.LowTempCelsius.HasValue)
			{
				template.LowTempKelvin = GameUtil.GetTemperatureConvertedToKelvin(template.LowTempCelsius.Value, GameUtil.TemperatureUnit.Celsius);
			}

			if (!template.DefaultTemperatureKelvin.HasValue && template.DefaultTemperatureCelsius.HasValue)
			{
				template.DefaultTemperatureKelvin = GameUtil.GetTemperatureConvertedToKelvin(template.DefaultTemperatureCelsius.Value, GameUtil.TemperatureUnit.Celsius);
			}
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
			Log.Debug("converting to entry " + template.Id);

			return new global::ElementLoader.ElementEntry()
			{
				elementId = template.Id,
				specificHeatCapacity = template.SpecificHeatCapacity.GetValueOrDefault(),
				thermalConductivity = template.ThermalConductivity.GetValueOrDefault(),
				solidSurfaceAreaMultiplier = template.SolidSurfaceAreaMultiplier.GetValueOrDefault(),
				liquidSurfaceAreaMultiplier = template.LiquidSurfaceAreaMultiplier.GetValueOrDefault(),
				gasSurfaceAreaMultiplier = template.GasSurfaceAreaMultiplier.GetValueOrDefault(),
				defaultMass = template.DefaultMass.GetValueOrDefault(),
				defaultTemperature = template.DefaultTemperatureKelvin.GetValueOrDefault(),
				defaultPressure = template.DefaultPressure.GetValueOrDefault(),
				molarMass = template.MolarMass.GetValueOrDefault(),
				lightAbsorptionFactor = template.LightAbsorptionFactor.GetValueOrDefault(),
				radiationAbsorptionFactor = template.RadiationAbsorptionFactor.GetValueOrDefault(),
				radiationPer1000Mass = template.RadiationPer1000Mass.GetValueOrDefault(),
				lowTempTransitionTarget = template.LowTempTransitionTarget,
				lowTemp = template.LowTempKelvin.GetValueOrDefault(),
				highTempTransitionTarget = template.HighTempTransitionTarget,
				highTemp = template.HighTempKelvin.GetValueOrDefault(),
				lowTempTransitionOreId = template.LowTempTransitionOreId,
				lowTempTransitionOreMassConversion = template.LowTempTransitionOreMassConversion.GetValueOrDefault(),
				highTempTransitionOreId = template.HighTempTransitionOreId,
				highTempTransitionOreMassConversion = template.HighTempTransitionOreMassConversion.GetValueOrDefault(),
				sublimateId = template.SublimateId,
				sublimateFx = template.SublimateFx,
				sublimateRate = template.SublimateRate.GetValueOrDefault(),
				sublimateEfficiency = template.SublimateEfficiency.GetValueOrDefault(),
				sublimateProbability = template.SublimateProbability.GetValueOrDefault(),
				offGasPercentage = template.OffGasPercentage.GetValueOrDefault(),
				materialCategory = template.MaterialCategory,
				tags = template.Tags,
				isDisabled = template.IsDisabled.GetValueOrDefault(),
				strength = template.Strength.GetValueOrDefault(),
				maxMass = template.MaxMass.GetValueOrDefault(),
				hardness = template.Hardness.GetValueOrDefault(),
				toxicity = template.Toxicity.GetValueOrDefault(),
				liquidCompression = template.LiquidCompression.GetValueOrDefault(),
				speed = template.Speed.GetValueOrDefault(),
				minHorizontalFlow = template.MinHorizontalFlow.GetValueOrDefault(),
				minVerticalFlow = template.MinVerticalFlow.GetValueOrDefault(),
				convertId = template.ConvertId,
				flow = template.Flow.GetValueOrDefault(),
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
