using HarmonyLib;
using Klei.AI;
using Moonlet.Scripts;
using Moonlet.Templates;
using Moonlet.Utils;
using System;
using System.Collections.Generic;
using System.IO;
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

		public void LoadContent(ref Dictionary<string, SubstanceTable> substanceTables)
		{
			var insertIntoTable = substanceTables[DlcManager.VANILLA_ID];

			Dictionary<string, Substance> lookup = [];
			foreach (var table in substanceTables.Values)
			{
				foreach (var item in table.GetList())
					lookup[item.elementID.ToString()] = item;
			}

			var substances = insertIntoTable.GetList();

			CreateSubstance(ref substances, lookup);
			ConfigureRottableAtmosphere();

			Moonlet_Mod.stepOnEffects ??= [];
			Moonlet_Mod.stepOnEffects[elementInfo.SimHash] = template.DuplicantEffects;
		}

		public void SetExposureValue(Dictionary<SimHashes, float> customExposureRates)
		{
			if (template.EyeIrritationStrength != null)
				customExposureRates[elementInfo.SimHash] = template.EyeIrritationStrength.Calculate();
		}

		public void CreateInfo()
		{
			var anim = GetElementAnim();
			elementInfo = new ElementInfo(template.Id, anim, template.State, (Color)template.Color, !isOverridingVanillaContent);
		}

		private void CreateSubstance(ref List<Substance> substances, Dictionary<string, Substance> lookup)
		{
			Log.Debug("creating substance for " + template.Id);

			oreMaterial ??= lookup[SimHashes.Cuprite.ToString()].material;
			refinedMaterial ??= lookup[SimHashes.Copper.ToString()].material;
			gemMaterial ??= lookup[SimHashes.Diamond.ToString()].material;

			var element = template;

			var uiColor = element.UiColor;
			var conduitColor = element.ConduitColor;
			var specularColor = element.SpecularColor == null ? Color.black : (Color)element.SpecularColor;

			var specular = !element.SpecularTexture.IsNullOrWhiteSpace();

			var path = MoonletMods.Instance.GetAssetsPath(sourceMod, "elements");

			Substance substance;

			if (!isOverridingVanillaContent)
			{
				Log.Debug("setting substance of " + template.Id);

				var material = GetElementMaterial(substances);

				substance = elementInfo.CreateSubstance(path, specular, material, uiColor, conduitColor, specularColor, element.NormalMapTexture);

				var uvScale = element.TextureUVScale.CalculateOrDefault(0);
				if (uvScale != 0)
					substance.material.SetFloat("_WorldUVScale", uvScale);

				if (substance.anim == null)
					Log.Debug("substance anim is null >:(");
				else
					Log.Debug(substance.anim.name);
			}
			else
			{
				if (!lookup.TryGetValue(template.Id, out substance))
					Warn("Trying to override an element but the original does not have a substance associated.");
				else
				{
					if (element.Color != null)
						substance.colour = element.Color.value;

					if (element.UiColor != null)
						substance.uiColour = element.UiColor.value;

					if (element.ConduitColor != null)
						substance.conduitColour = element.ConduitColor.value;

					if (element.SpecularColor != null)
						substance.material.SetColor("_ShineColour", element.SpecularColor.value);

					if (element.SpecularTexture != null)
						ElementUtil.SetTexture(substance.material, Path.Combine(path, element.SpecularTexture), "_ShineMask");

					if (element.NormalMapTexture != null)
						ElementUtil.SetTexture(substance.material, Path.Combine(path, element.NormalMapTexture), "_NormalNoise");

					if (element.TextureUVScale != null)
						substance.material.SetFloat("_WorldUVScale", element.TextureUVScale);

					if (!element.DebrisAnim.IsNullOrWhiteSpace())
						substance.anim = Assets.GetAnim(element.DebrisAnim);
				}
			}

			if (element.MainTextureFromExisting != null)
			{
				var reference = substances.Find(s => s.elementID.ToString() == element.MainTextureFromExisting);
				if (reference != null)
				{
					substance.material.SetTexture("_MainTex", reference.material.GetTexture("_MainTex"));
					substance.material.SetFloat("_WorldUVScale", reference.material.GetFloat("_WorldUVScale"));
				}
				else
					Warn($"Main Texture path set to copy {element.MainTextureFromExisting}, but it does not exist.");
			}
			else if (element.MainTexture != null)
			{
				var texPath = Path.Combine(path, element.MainTexture);
				if (File.Exists(texPath))
					ElementUtil.SetTexture(substance.material, texPath, "_MainTex");
				else
					Warn($"Main Texture path set at {texPath}, but it does not exist.");
			}

			if (!isOverridingVanillaContent)
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

		public override void Initialize()
		{
			if (Enum.GetNames(typeof(SimHashes)).Contains(template.Id))
				isOverridingVanillaContent = true;

			base.Initialize();
		}

		public override void Validate()
		{
			base.Validate();

			if (isOverridingVanillaContent)
				return;

			if (!template.Name.StartsWith("<link"))
				FormatAsLink(template.Name, template.Id.ToUpperInvariant());

			template.Name = FUtility.Utils.FormatAsLink(template.Name, template.Id.ToUpperInvariant());
			nameKey = $"STRINGS.ELEMENTS.{template.Id.ToUpperInvariant()}.NAME";
			descriptionKey = $"STRINGS.ELEMENTS.{template.Id.ToUpperInvariant()}.DESCRIPTION";

			Strings.Add(nameKey, template.Name);
			Strings.Add(descriptionKey, template.DescriptionText);

			if (template.Color == null || !template.Color.hasValue)
			{
				Log.Warn($"{template.Id} has no color defined!");
				template.Color = new ColorEntry(Color.white);
			}

			template.ConduitColor ??= template.Color.value;
			template.UiColor ??= template.Color.value;

			if (!template.DlcId.IsNullOrWhiteSpace() && template.DlcIds == null)
			{
				Warn("DlcId is deprecated. Please use the plural version DlcIds.");
				template.DlcIds = [template.DlcId];
			}

			template.DlcIds ??= DlcManager.AVAILABLE_ALL_VERSIONS;

			if (DlcManager.IsDlcListValidForCurrentContent(template.DlcIds))
			{
				template.DlcId = DlcManager.VANILLA_ID;
			}
			else
			{
				foreach (var dlcId in DlcManager.GetActiveDLCIds())
				{
					if (!template.DlcIds.Contains(dlcId))
						template.DlcId = dlcId;
				}
			}

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
				solidSurfaceAreaMultiplier = template.SolidSurfaceAreaMultiplier.CalculateOrDefault(GetDefaultSolidSurfMult(template.State)),
				liquidSurfaceAreaMultiplier = template.LiquidSurfaceAreaMultiplier.CalculateOrDefault(GetDefaultLiquidSurfMult(template.State)),
				gasSurfaceAreaMultiplier = template.GasSurfaceAreaMultiplier.CalculateOrDefault(GetDefaultGasSurfMult(template.State)),
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
				hardness = (byte)template.Hardness.CalculateOrDefault(0),
				toxicity = template.Toxicity.CalculateOrDefault(),
				liquidCompression = template.LiquidCompression.CalculateOrDefault(),
				speed = template.Speed.CalculateOrDefault(),
				minHorizontalFlow = template.MinHorizontalFlow.CalculateOrDefault(),
				minVerticalFlow = template.MinVerticalFlow.CalculateOrDefault(),
				convertId = template.ConvertId,
				flow = template.Flow.CalculateOrDefault(),
				buildMenuSort = template.BuildMenuSort.CalculateOrDefault(),
				state = template.State,
				localizationID = nameKey,
				dlcId = template.DlcId,
				composition = template.Composition,
				description = descriptionKey,
			};
		}

		private float GetDefaultSolidSurfMult(Element.State state) => state == Element.State.Gas ? 25 : 1;

		private float GetDefaultLiquidSurfMult(Element.State state) => state == Element.State.Liquid ? 25 : 1;

		private float GetDefaultGasSurfMult(Element.State state) => 1;

		public void ApplyModifiers()
		{
			if (template.Modifiers == null)
				return;

			var element = elementInfo.Get();
			if (element == null)
				return;

			foreach (var modifier in template.Modifiers)
			{
				if (modifier.Value == 0)
					continue;

				if (Db.Get().BuildingAttributes.TryGet(modifier.Id) == null)
				{
					if (!modifier.Optional)
						Log.Error($"Incorrect modifier ID: {modifier.Id}. Fix the ID or mark the entry as an Optional modifier.");

					continue;
				}

				element.attributeModifiers.Add(new AttributeModifier(modifier.Id, modifier.Value.Calculate(), element.name, modifier.IsMultiplier));
			}
		}

		public bool IsMetal()
		{
			return template.MaterialCategory == "Metal"
				|| template.MaterialCategory == "RefinedMetal"
				|| (template.Tags != null && template.Tags.Contains("Metal"));
		}

		public void LoadAudioConfigs(ElementsAudio elementsAudio)
		{
			Log.Debug("loading audio for " + template.Id);

			if (template.Audio == null)
				return;

			Log.Debug("has audio");

			var config = new ElementsAudio.ElementAudioConfig()
			{
				elementID = ElementUtil.GetSimhashSafe(template.Id)
			};

			var baseElement = template.Audio.CopyElement;

			if (baseElement.IsNullOrWhiteSpace())
			{
				if (template.Tags != null)
				{
					if (template.Tags.Contains(GameTags.Ore.ToString()))
						baseElement = SimHashes.Cuprite.ToString();
					else if (template.Tags.Contains(GameTags.Metal.ToString()))
						baseElement = SimHashes.Copper.ToString();
					else if (template.Tags.Contains(GameTags.Organics.ToString()) || template.MaterialCategory == GameTags.Organics.ToString())
						baseElement = SimHashes.Algae.ToString();
					else if (template.Tags.Contains(GameTags.Unstable.ToString()))
						baseElement = SimHashes.Sand.ToString();
					else
						baseElement = SimHashes.SandStone.ToString();
				}
				else
					baseElement = SimHashes.SandStone.ToString();
			}

			var simHash = ElementUtil.GetSimhashSafe(baseElement);

			if (simHash == SimHashes.Void)
			{
				Warn($"Cannot copy element audio of \"{template.Audio.CopyElement}\", this is not a registered simhash.");
				simHash = SimHashes.SandStone;
			}
			else
			{
				Log.Debug("copying audio config: " + baseElement);

				var copy = ElementUtil.CopyElementAudioConfig(simHash, elementInfo.SimHash);
				if (copy != null)
					config = copy;
			}

			if (template.Audio.AmbienceType != null)
			{
				var ambienceType = EnumUtils.ParseOrDefault(template.Audio.AmbienceType, AmbienceType.None);
				config.ambienceType = ambienceType;
			}

			if (template.Audio.SolidAmbienceType != null)
			{
				var solidAmbienceType = EnumUtils.ParseOrDefault(template.Audio.SolidAmbienceType, SolidAmbienceType.None);
				config.solidAmbienceType = solidAmbienceType;
			}

			if (!template.Audio.MiningSound.IsNullOrWhiteSpace())
				config.miningSound = template.Audio.MiningSound;

			if (!template.Audio.MiningBreakSound.IsNullOrWhiteSpace())
				config.miningBreakSound = template.Audio.MiningBreakSound;

			if (!template.Audio.OreBumpSound.IsNullOrWhiteSpace())
				config.oreBumpSound = template.Audio.OreBumpSound;

			if (!template.Audio.FloorEventAudioCategory.IsNullOrWhiteSpace())
				config.floorEventAudioCategory = template.Audio.FloorEventAudioCategory;

			if (!template.Audio.CreatureChewSound.IsNullOrWhiteSpace())
				config.creatureChewSound = template.Audio.CreatureChewSound;

			elementsAudio.elementAudioConfigs = elementsAudio.elementAudioConfigs.AddToArray(config);
		}

		public void ApplyToOriginal(ref List<global::ElementLoader.ElementEntry> entries)
		{
			var original = entries.Find(entry => entry.elementId == template.Id);
			if (original == null)
			{
				Warn("trying to override an element but it doesnt exist.");
				return;
			}

			if (template.SpecificHeatCapacity != null)
				original.specificHeatCapacity = template.SpecificHeatCapacity;
			if (template.ThermalConductivity != null)
				original.thermalConductivity = template.ThermalConductivity;
			if (template.SolidSurfaceAreaMultiplier != null)
				original.solidSurfaceAreaMultiplier = template.SolidSurfaceAreaMultiplier.CalculateOrDefault(GetDefaultSolidSurfMult(template.State));
			if (template.LiquidSurfaceAreaMultiplier != null)
				original.liquidSurfaceAreaMultiplier = template.LiquidSurfaceAreaMultiplier.CalculateOrDefault(GetDefaultLiquidSurfMult(template.State));
			if (template.GasSurfaceAreaMultiplier != null)
				original.gasSurfaceAreaMultiplier = template.GasSurfaceAreaMultiplier.CalculateOrDefault(GetDefaultGasSurfMult(template.State));
			if (template.DefaultMass != null)
				original.defaultMass = template.DefaultMass;
			if (template.DefaultTemperature != null)
				original.defaultTemperature = template.DefaultTemperature;
			if (template.DefaultPressure != null)
				original.defaultPressure = template.DefaultPressure;
			if (template.MolarMass != null)
				original.molarMass = template.MolarMass;
			if (template.LightAbsorptionFactor != null)
				original.lightAbsorptionFactor = template.LightAbsorptionFactor;
			if (template.RadiationAbsorptionFactor != null)
				original.radiationAbsorptionFactor = template.RadiationAbsorptionFactor;
			if (template.RadiationPer1000Mass != null)
				original.radiationPer1000Mass = template.RadiationPer1000Mass;
			if (template.LowTempTransitionTarget != null)
				original.lowTempTransitionTarget = template.LowTempTransitionTarget;
			if (template.LowTemp != null)
				original.lowTemp = template.LowTemp;
			if (template.HighTempTransitionTarget != null)
				original.highTempTransitionTarget = template.HighTempTransitionTarget;
			if (template.HighTemp != null)
				original.highTemp = template.HighTemp;
			if (template.LowTempTransitionOreId != null)
				original.lowTempTransitionOreId = template.LowTempTransitionOreId;
			if (template.LowTempTransitionOreMassConversion != null)
				original.lowTempTransitionOreMassConversion = template.LowTempTransitionOreMassConversion;
			if (template.HighTempTransitionOreId != null)
				original.highTempTransitionOreId = template.HighTempTransitionOreId;
			if (template.HighTempTransitionOreMassConversion != null)
				original.highTempTransitionOreMassConversion = template.HighTempTransitionOreMassConversion;
			if (template.SublimateId != null)
				original.sublimateId = template.SublimateId;
			if (template.SublimateFx != null)
				original.sublimateFx = template.SublimateFx;
			if (template.SublimateRate != null)
				original.sublimateRate = template.SublimateRate;
			if (template.SublimateEfficiency != null)
				original.sublimateEfficiency = template.SublimateEfficiency;
			if (template.SublimateProbability != null)
				original.sublimateProbability = template.SublimateProbability;
			if (template.OffGasPercentage != null)
				original.offGasPercentage = template.OffGasPercentage;
			if (template.MaterialCategory != null)
				original.materialCategory = template.MaterialCategory;
			if (template.Tags != null)
			{
				original.tags ??= [];
				original.tags = original.tags.AddRangeToArray(template.Tags);
			}
			if (template.IsDisabled != null)
				original.isDisabled = template.IsDisabled.GetValueOrDefault();
			if (template.Strength != null)
				original.strength = template.Strength;
			if (template.MaxMass != null)
				original.maxMass = template.MaxMass;
			if (template.Hardness != null)
				original.hardness = (byte)template.Hardness.CalculateOrDefault(0);
			if (template.Toxicity != null)
				original.toxicity = template.Toxicity;
			if (template.LiquidCompression != null)
				original.liquidCompression = template.LiquidCompression;
			if (template.Speed != null)
				original.speed = template.Speed;
			if (template.MinHorizontalFlow != null)
				original.minHorizontalFlow = template.MinHorizontalFlow;
			if (template.MinVerticalFlow != null)
				original.minVerticalFlow = template.MinVerticalFlow;
			if (template.ConvertId != null)
				original.convertId = template.ConvertId;
			if (template.Flow != null)
				original.flow = template.Flow;
			if (template.BuildMenuSort != null)
				original.buildMenuSort = template.BuildMenuSort;
			if (template.Composition != null)
				original.composition = template.Composition;

			if (template.RemoveTags != null && original.tags != null)
			{
				var tags = new List<string>(original.tags)
					.RemoveAll(tag => template.RemoveTags.Contains(tag));
			}
		}
	}
}
