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

		public void LoadContent(ref List<Substance> substances)
		{
			CreateSubstance(substances);
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
			var element = template;
			var anim = GetElementAnim();
			var color = element.Color;

			elementInfo = new ElementInfo(element.Id, anim, element.State, (Color)color);
		}

		private void CreateSubstance(List<Substance> substances)
		{
			oreMaterial ??= substances.Find(e => e.elementID == SimHashes.Cuprite).material;
			refinedMaterial ??= substances.Find(e => e.elementID == SimHashes.Copper).material;
			gemMaterial ??= substances.Find(e => e.elementID == SimHashes.Diamond).material;

			var element = template;

			var uiColor = element.UiColor;
			var conduitColor = element.ConduitColor;
			var specularColor = element.SpecularColor == null ? Color.black : (Color)element.SpecularColor;

			var specular = !element.SpecularTexture.IsNullOrWhiteSpace();
			var material = GetElementMaterial(substances);

			var path = MoonletMods.Instance.GetAssetsPath(sourceMod, "elements");

			var substance = elementInfo.CreateSubstance(path, specular, material, uiColor, conduitColor, specularColor, element.NormalMapTexture);

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

			var uvScale = element.TextureUVScale.CalculateOrDefault(0);
			if (uvScale != 0)
				substance.material.SetFloat("_WorldUVScale", uvScale);

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
				elementID = elementInfo.SimHash
			};

			var baseElement = template.Audio.CopyElement;

			if (baseElement.IsNullOrWhiteSpace())
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
	}
}
