using FUtility;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using YamlDotNet.Serialization;
using static ElementLoader;

namespace Moonlet.Elements
{
	public class ExtendedElementEntry
	{
		[YamlIgnore] private string description_backing;
		[YamlIgnore] public int? priority;
		[YamlIgnore] public bool invalid; // used for technical entries, like default files
		[YamlIgnore] public List<string> addedBy;
		[YamlIgnore] public string textureFolder;
		[YamlIgnore] public SimHashes simHash;

		[YamlIgnore]
		public bool IsAlwaysRequired => DefaultPriority == Priority.Required.ToString();

		public string ElementId { get; set; }

		public float? SpecificHeatCapacity { get; set; }

		public float? ThermalConductivity { get; set; }

		public float? SolidSurfaceAreaMultiplier { get; set; }

		public float? LiquidSurfaceAreaMultiplier { get; set; }

		public float? GasSurfaceAreaMultiplier { get; set; }

		public float? DefaultMass { get; set; }

		[YamlMember(Alias = "defaultTemperature")]
		public float? DefaultTemperatureKelvin { get; set; }

		public float? DefaultPressure { get; set; }

		public float? MolarMass { get; set; }

		public float? LightAbsorptionFactor { get; set; }

		public float? RadiationAbsorptionFactor { get; set; }

		public float? RadiationPer1000Mass { get; set; }

		public string LowTempTransitionTarget { get; set; }

		[YamlMember(Alias = "lowTemp")]
		public float? LowTempKelvin { get; set; }

		public string HighTempTransitionTarget { get; set; }

		[YamlMember(Alias = "highTemp")]
		public float? HighTempKelvin { get; set; }

		public string LowTempTransitionOreId { get; set; }

		public float? LowTempTransitionOreMassConversion { get; set; }

		public string HighTempTransitionOreId { get; set; }

		public float? HighTempTransitionOreMassConversion { get; set; }

		public string SublimateId { get; set; }

		public string SublimateFx { get; set; }

		public float? SublimateRate { get; set; }

		public float? SublimateEfficiency { get; set; }

		public float? SublimateProbability { get; set; }

		public float? OffGasPercentage { get; set; }

		public string MaterialCategory { get; set; }

		public string[] Tags { get; set; }

		public bool? IsDisabled { get; set; }

		public float? Strength { get; set; }

		public float? MaxMass { get; set; }

		public byte? Hardness { get; set; }

		public float? Toxicity { get; set; }

		public float? LiquidCompression { get; set; }

		public float? Speed { get; set; }

		public float? MinHorizontalFlow { get; set; }

		public float? MinVerticalFlow { get; set; }

		public string ConvertId { get; set; }

		public float? Flow { get; set; }

		public int? BuildMenuSort { get; set; }

		public Element.State State { get; set; }

		public string LocalizationID { get; set; }

		public string DlcId { get; set; }

		public ElementComposition[] Composition { get; set; }

		public string Description
		{
			get => description_backing ?? "STRINGS.ELEMENTS." + this.ElementId.ToString().ToUpper() + ".DESC";
			set => description_backing = value;
		}

		// end of original fields ===========================================

		// keeping properties lower case for consistency with base game yaml
		public string DefaultPriority { get; set; }

		public PrioritySetting[] PriorityPerCluster { get; set; }

		public string Name { get; set; }

		public string[] RequiredMods { get; set; }

		public string DescriptionText { get; set; }

		public string Color { get; set; }

		public string UiColor { get; set; }

		public string ConduitColor { get; set; }

		public string UnstableColorTint { get; set; }

		public string MaterialReference { get; set; }

		public string MainTexture { get; set; }

		public string MainTextureFromExisting { get; set; }

		public string MainTextureTint { get; set; }

		public string SpecularTexture { get; set; }

		public float? TextureUVScale { get; set; }

		public string NormalMapTexture { get; set; }

		public string SpecularColor { get; set; }

		public string DebrisAnim { get; set; }

		public string RotAtmosphereQuality { get; set; }

		public string GasTextureType { get; set; }

		public string UnstableAnim { get; set; }

		public float? EyeIrritationStrength { get; set; }

		public ModifierData Modifiers { get; set; }

		public Effects DuplicantEffects { get; set; }

		public AudioConfig Audio { get; set; }

		public float? DefaultTemperatureCelsius { get; set; }
		public float? HighTempCelsius { get; set; }
		public float? LowTempCelsius { get; set; }


		// wildcard
		public List<CustomDataEntry> CustomData { get; set; }

		public void ApplyOverride(ElementEntry entry)
		{
			Log.Debuglog("Applying override to " + entry.elementId);

			if (DlcId != null)
			{
				Log.Warning($"{ElementId} override: DLC Requirement cannot be overridden");
				return;
			}

			if (ElementId != null)
			{
				Log.Warning($"{ElementId} elementId cannot be overridden");
				return;
			}

			if (ConvertId != null)
			{
				Log.Warning($"{ElementId} convertId cannot be overridden");
				return;
			}

			if (entry.dlcId != null && !DlcManager.IsContentActive(entry.dlcId))
				return;

			entry.specificHeatCapacity = SpecificHeatCapacity.HasValue ? SpecificHeatCapacity.GetValueOrDefault() : entry.specificHeatCapacity;
			entry.thermalConductivity = ThermalConductivity.HasValue ? ThermalConductivity.GetValueOrDefault() : entry.thermalConductivity;
			entry.solidSurfaceAreaMultiplier = SolidSurfaceAreaMultiplier.HasValue ? SolidSurfaceAreaMultiplier.GetValueOrDefault() : entry.solidSurfaceAreaMultiplier;
			entry.liquidSurfaceAreaMultiplier = LiquidSurfaceAreaMultiplier.HasValue ? LiquidSurfaceAreaMultiplier.GetValueOrDefault() : entry.liquidSurfaceAreaMultiplier;
			entry.gasSurfaceAreaMultiplier = GasSurfaceAreaMultiplier.HasValue ? GasSurfaceAreaMultiplier.GetValueOrDefault() : entry.gasSurfaceAreaMultiplier;
			entry.defaultMass = DefaultMass.HasValue ? DefaultMass.GetValueOrDefault() : entry.defaultMass;
			entry.defaultTemperature = DefaultTemperatureKelvin.HasValue ? DefaultTemperatureKelvin.GetValueOrDefault() : entry.defaultTemperature;
			entry.defaultPressure = DefaultPressure.HasValue ? DefaultPressure.GetValueOrDefault() : entry.defaultPressure;
			entry.molarMass = MolarMass.HasValue ? MolarMass.GetValueOrDefault() : entry.molarMass;
			entry.lightAbsorptionFactor = LightAbsorptionFactor.HasValue ? LightAbsorptionFactor.GetValueOrDefault() : entry.lightAbsorptionFactor;
			entry.radiationAbsorptionFactor = RadiationAbsorptionFactor.HasValue ? RadiationAbsorptionFactor.GetValueOrDefault() : entry.radiationAbsorptionFactor;
			entry.radiationPer1000Mass = RadiationPer1000Mass.HasValue ? RadiationPer1000Mass.GetValueOrDefault() : entry.radiationPer1000Mass;
			entry.lowTempTransitionTarget = LowTempTransitionTarget ?? entry.lowTempTransitionTarget;
			entry.lowTemp = LowTempKelvin.HasValue ? LowTempKelvin.GetValueOrDefault() : entry.lowTemp;
			entry.highTempTransitionTarget = HighTempTransitionTarget ?? entry.highTempTransitionTarget;
			entry.highTemp = HighTempKelvin.HasValue ? HighTempKelvin.GetValueOrDefault() : entry.highTemp;
			entry.lowTempTransitionOreId = LowTempTransitionOreId ?? entry.lowTempTransitionOreId;
			entry.lowTempTransitionOreMassConversion = LowTempTransitionOreMassConversion.HasValue ? LowTempTransitionOreMassConversion.GetValueOrDefault() : entry.lowTempTransitionOreMassConversion;
			entry.highTempTransitionOreId = HighTempTransitionOreId ?? entry.highTempTransitionOreId;
			entry.highTempTransitionOreMassConversion = HighTempTransitionOreMassConversion.HasValue ? HighTempTransitionOreMassConversion.GetValueOrDefault() : entry.highTempTransitionOreMassConversion;
			entry.sublimateId = SublimateId ?? entry.sublimateId;
			entry.sublimateFx = SublimateFx ?? entry.sublimateFx;
			entry.sublimateRate = SublimateRate.HasValue ? SublimateRate.GetValueOrDefault() : entry.sublimateRate;
			entry.sublimateEfficiency = SublimateEfficiency.HasValue ? SublimateEfficiency.GetValueOrDefault() : entry.sublimateEfficiency;
			entry.sublimateProbability = SublimateProbability.HasValue ? SublimateProbability.GetValueOrDefault() : entry.sublimateProbability;
			entry.offGasPercentage = OffGasPercentage.HasValue ? OffGasPercentage.GetValueOrDefault() : entry.offGasPercentage;
			entry.materialCategory = MaterialCategory ?? entry.materialCategory;
			entry.tags = Tags ?? entry.tags;
			entry.isDisabled = IsDisabled.HasValue ? IsDisabled.GetValueOrDefault() : entry.isDisabled;
			entry.strength = Strength.HasValue ? Strength.GetValueOrDefault() : entry.strength;
			entry.maxMass = MaxMass.HasValue ? MaxMass.GetValueOrDefault() : entry.maxMass;
			entry.hardness = Hardness.HasValue ? Hardness.GetValueOrDefault() : entry.hardness;
			entry.toxicity = Toxicity.HasValue ? Toxicity.GetValueOrDefault() : entry.toxicity;
			entry.liquidCompression = LiquidCompression.HasValue ? LiquidCompression.GetValueOrDefault() : entry.liquidCompression;
			entry.speed = Speed.HasValue ? Speed.GetValueOrDefault() : entry.speed;
			entry.minHorizontalFlow = MinHorizontalFlow.HasValue ? MinHorizontalFlow.GetValueOrDefault() : entry.minHorizontalFlow;
			entry.minVerticalFlow = MinVerticalFlow.HasValue ? MinVerticalFlow.GetValueOrDefault() : entry.minVerticalFlow;
			entry.flow = Flow.HasValue ? Flow.GetValueOrDefault() : entry.flow;
			entry.buildMenuSort = BuildMenuSort.HasValue ? BuildMenuSort.GetValueOrDefault() : entry.buildMenuSort;
			entry.localizationID = LocalizationID ?? entry.localizationID;
			entry.composition = Composition ?? entry.composition;
			entry.description = Description ?? entry.description;
		}

		public ElementEntry CreateEntry()
		{
			return new ElementEntry()
			{
				elementId = ElementId,
				specificHeatCapacity = SpecificHeatCapacity.GetValueOrDefault(),
				thermalConductivity = ThermalConductivity.GetValueOrDefault(),
				solidSurfaceAreaMultiplier = SolidSurfaceAreaMultiplier.GetValueOrDefault(),
				liquidSurfaceAreaMultiplier = LiquidSurfaceAreaMultiplier.GetValueOrDefault(),
				gasSurfaceAreaMultiplier = GasSurfaceAreaMultiplier.GetValueOrDefault(),
				defaultMass = DefaultMass.GetValueOrDefault(),
				defaultTemperature = DefaultTemperatureKelvin.GetValueOrDefault(),
				defaultPressure = DefaultPressure.GetValueOrDefault(),
				molarMass = MolarMass.GetValueOrDefault(),
				lightAbsorptionFactor = LightAbsorptionFactor.GetValueOrDefault(),
				radiationAbsorptionFactor = RadiationAbsorptionFactor.GetValueOrDefault(),
				radiationPer1000Mass = RadiationPer1000Mass.GetValueOrDefault(),
				lowTempTransitionTarget = LowTempTransitionTarget,
				lowTemp = LowTempKelvin.GetValueOrDefault(),
				highTempTransitionTarget = HighTempTransitionTarget,
				highTemp = HighTempKelvin.GetValueOrDefault(),
				lowTempTransitionOreId = LowTempTransitionOreId,
				lowTempTransitionOreMassConversion = LowTempTransitionOreMassConversion.GetValueOrDefault(),
				highTempTransitionOreId = HighTempTransitionOreId,
				highTempTransitionOreMassConversion = HighTempTransitionOreMassConversion.GetValueOrDefault(),
				sublimateId = SublimateId,
				sublimateFx = SublimateFx,
				sublimateRate = SublimateRate.GetValueOrDefault(),
				sublimateEfficiency = SublimateEfficiency.GetValueOrDefault(),
				sublimateProbability = SublimateProbability.GetValueOrDefault(),
				offGasPercentage = OffGasPercentage.GetValueOrDefault(),
				materialCategory = MaterialCategory,
				tags = Tags,
				isDisabled = IsDisabled.GetValueOrDefault(),
				strength = Strength.GetValueOrDefault(),
				maxMass = MaxMass.GetValueOrDefault(),
				hardness = Hardness.GetValueOrDefault(),
				toxicity = Toxicity.GetValueOrDefault(),
				liquidCompression = LiquidCompression.GetValueOrDefault(),
				speed = Speed.GetValueOrDefault(),
				minHorizontalFlow = MinHorizontalFlow.GetValueOrDefault(),
				minVerticalFlow = MinVerticalFlow.GetValueOrDefault(),
				convertId = ConvertId,
				flow = Flow.GetValueOrDefault(),
				buildMenuSort = BuildMenuSort.GetValueOrDefault(),
				state = State,
				localizationID = LocalizationID,
				dlcId = DlcId,
				composition = Composition,
				description = Description,
			};
		}

		public const float KELVIN = 273.15f;

		public void Validate()
		{
			if (Color.IsNullOrWhiteSpace()) Color = "FFFFFF";
			if (UiColor.IsNullOrWhiteSpace()) UiColor = Color;
			if (ConduitColor.IsNullOrWhiteSpace()) ConduitColor = Color;

			DlcId ??= DlcManager.VANILLA_ID;

			if (HighTempCelsius.HasValue)
				HighTempKelvin = HighTempCelsius.Value + KELVIN;

			if (!HighTempKelvin.HasValue)
				HighTempKelvin = 9999;

			if (LowTempCelsius.HasValue)
				LowTempKelvin = LowTempCelsius.Value + KELVIN;

			if (DefaultTemperatureCelsius.HasValue)
				DefaultTemperatureKelvin = DefaultTemperatureCelsius.Value + KELVIN;

			if (ElementId.IsNullOrWhiteSpace())
				Log.Warning($"Element must have an elementId defined!");

			if (HighTempKelvin.HasValue && HighTempKelvin > 9999)
			{
				Log.Warning($"Element {ElementId} has a highTemp over 9999. This would result in a Sim dll crash. Clamped to 9999.");
				HighTempKelvin = 9999;
			}

			if (LowTempKelvin.HasValue && LowTempKelvin < 0)
			{
				Log.Warning($"Element {ElementId} has a lowTemp below absolute 0. This would result in a Sim dll crash. Clamped to 0.");
				LowTempKelvin = 0;
			}

			if (ThermalConductivity > 2000)
				Log.Warning($"Element {ElementId} has an unusually high Thermal Conductivity. The sim may not be accurate.");

			if (EyeIrritationStrength.HasValue && (EyeIrritationStrength > 1 || EyeIrritationStrength < 0))
			{
				Log.Warning($"Element {ElementId}: eyeIrritationStrength is expected to be 0-1.");
				EyeIrritationStrength = Mathf.Clamp01(EyeIrritationStrength.Value);
			}

			if (RotAtmosphereQuality != null && !Enum.TryParse<Rottable.RotAtmosphereQuality>(RotAtmosphereQuality, out _))
			{
				Log.Warning($"Element {ElementId}: rotAtmosphereQuality \"{RotAtmosphereQuality}\" " +
					$"is invalid. Accepted values: {Enum.GetNames(typeof(Rottable.RotAtmosphereQuality)).Join()}");

				RotAtmosphereQuality = Rottable.RotAtmosphereQuality.Normal.ToString();
			}

			DefaultMass = Mathf.Max(0, DefaultMass.GetValueOrDefault());
			MaxMass = Mathf.Max(0, DefaultMass.GetValueOrDefault());
			DefaultTemperatureKelvin = Mathf.Clamp(0, 9999, DefaultMass.GetValueOrDefault());

			if (HighTempTransitionOreId != null && !HighTempTransitionOreMassConversion.HasValue)
				Log.Warning($"Element {ElementId} has a highTempTransitionOreId defined, but no highTempTransitionOreMassConversion.");

			if (LowTempTransitionOreId != null && !LowTempTransitionOreMassConversion.HasValue)
				Log.Warning($"Element {ElementId} has a lowTempTransitionOreId defined, but no lowTempTransitionOreMassConversion.");
		}

		public void AddStrings()
		{
			LocalizationID ??= $"STRINGS.ELEMENTS.{ElementId.ToUpperInvariant()}.NAME";
			Description ??= $"STRINGS.ELEMENTS.{ElementId.ToUpperInvariant()}.DESC";

			ModLoader.locstringKeys[LocalizationID] = Name ?? "";

			if (!Name.IsNullOrWhiteSpace())
				Strings.Add(LocalizationID, Name);

			ModLoader.locstringKeys[Description] = DescriptionText ?? "";

			if (!DescriptionText.IsNullOrWhiteSpace())
				Strings.Add(Description, DescriptionText);
		}

		public class CustomDataEntry
		{
			public string id;
			public object content;
		}

		public int GetDefaultPriority()
		{
			if (!priority.HasValue)
				priority = GetPriority(DefaultPriority);

			return priority.Value;
		}

		public int GetPriority(string value)
		{
			if (value.IsNullOrWhiteSpace())
				return (int)Priority.Default;

			if (Enum.TryParse<Priority>(value, out var enumValue))
				return (int)enumValue;

			if (int.TryParse(value, out var intValue))
				return intValue;

			Log.Warning($"Error in element {ElementId}: Priority value must be a valid keyword or integer number. " +
				$"Keywords: {Enum.GetNames(typeof(Priority)).Join()}");

			return (int)Priority.Default;
		}

		public object GetPriorityForCluster(string clusterId)
		{
			if (PriorityPerCluster != null && clusterId != null)
			{
				foreach (var cluster in PriorityPerCluster)
				{
					if (cluster.name == clusterId)
						return GetPriority(cluster.priority);
				}
			}

			return GetDefaultPriority();
		}

		public class AudioConfig
		{
			public string copyElement { get; set; }

			public string ambienceType { get; set; }

			public string solidAmbienceType { get; set; }

			public string miningSound { get; set; }

			public string miningBreakSound { get; set; }

			public string oreBumpSound { get; set; }

			public string floorEventAudioCategory { get; set; }

			public string creatureChewSound { get; set; }
		}

		public class Effects
		{
			public string soaked { get; set; }

			public bool soakedExlusive { get; set; }

			public string steppedIn { get; set; }

			public bool steppedInExclusive { get; set; }

			public string breathIn { get; set; }

			public string walkOn { get; set; }

		}

		public class PrioritySetting
		{
			public string name { get; set; }

			public string priority { get; set; }
		}

		public class ModifierData
		{
			public float? decor { get; set; }

			public float? overHeat { get; set; }
		}

		public static class GasTextureTypes
		{
			public const string
				DEFAULT = "Default",
				OXYGEN = "Oxygen";
		}

		public enum Priority
		{
			VeryLow = 0,
			Low = 5,
			Default = 10,
			High = 15,
			VeryHigh = 20,
			Required = 1000,
		}
	}
}
