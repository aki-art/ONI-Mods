using FUtility;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Moonlet.Elements
{
	public class ElementUtil
	{
		public static readonly Dictionary<SimHashes, string> SimHashNameLookup = new();
		public static readonly Dictionary<string, object> ReverseSimHashNameLookup = new();
		public static readonly List<ElementInfo> elements = new();

		public static SimHashes RegisterSimHash(string name)
		{
			var simHash = (SimHashes)Hash.SDBMLower(name);
			SimHashNameLookup.Add(simHash, name);
			ReverseSimHashNameLookup.Add(name, simHash);

			return simHash;
		}

		public static Substance CreateSubstance(SimHashes id, bool specular, string assetsPath, string anim, Element.State state, Color color, Material material, Color uiColor, Color conduitColor, Color? specularColor, string normal)
		{
			Log.Assert("ElementUtil.CreateSubstance.material", material);

			var animFile = Assets.Anims.Find(a => a.name == anim) 
				?? Assets.Anims.Find(a => a.name == "glass_kanim");
			var newMaterial = new Material(material);
			var stringId = SimHashNameLookup.TryGetValue(id, out var name) ? name : "";

			if(stringId.IsNullOrWhiteSpace())
			{
				Log.Warning("ID error");
				return null;
			}

			if (state == Element.State.Solid)
			{
				var mainPath = Path.Combine(assetsPath, stringId.ToLowerInvariant() + ".png");
				SetTexture(newMaterial, mainPath, "_MainTex");

				if (specular)
				{
					var specPath = Path.Combine(assetsPath, stringId.ToLowerInvariant() + "_spec.png");
					SetTexture(newMaterial, specPath, "_ShineMask");

					if (specularColor.HasValue)
						newMaterial.SetColor("_ShineColour", specularColor.Value);
				}

				if (!normal.IsNullOrWhiteSpace())
					SetTexture(newMaterial, normal, "_NormalNoise");
			}

			var substance = ModUtil.CreateSubstance(stringId, state, animFile, newMaterial, color, uiColor, conduitColor);

			return substance;
		}

		private static void SetTexture(Material material, string texturePath, string property)
		{
			if (FUtility.Assets.TryLoadTexture(texturePath, out var tex))
				material.SetTexture(property, tex);
			else
				Log.Warning("Could not load texture for " + texturePath);
		}

		// The game incorrectly assigns the display name to elements not in the original SimHashes table,
		// so this needs to be changed to the actual ID. 
		public static void FixTags()
		{
			foreach (var elem in elements)
			{
				elem.Get().substance.nameTag = TagManager.Create(elem.id);
			}
		}

		public static ElementsAudio.ElementAudioConfig CopyElementAudioConfig(SimHashes referenceId, SimHashes id)
		{
			var reference = ElementsAudio.Instance.GetConfigForElement(referenceId);

			return new ElementsAudio.ElementAudioConfig()
			{
				elementID = reference.elementID,
				ambienceType = reference.ambienceType,
				solidAmbienceType = reference.solidAmbienceType,
				miningSound = reference.miningSound,
				miningBreakSound = reference.miningBreakSound,
				oreBumpSound = reference.oreBumpSound,
				floorEventAudioCategory = reference.floorEventAudioCategory,
				creatureChewSound = reference.creatureChewSound,
			};
		}

		public static ElementLoader.ElementEntry Convert(ElementData data)
		{
			var dlc = data.dlcId ?? DlcManager.VANILLA_ID;

			return new ElementLoader.ElementEntry()
			{
				elementId = data.elementId,
				specificHeatCapacity = data.specificHeatCapacity,
				thermalConductivity = data.thermalConductivity,
				solidSurfaceAreaMultiplier = data.solidSurfaceAreaMultiplier,
				liquidSurfaceAreaMultiplier = data.liquidSurfaceAreaMultiplier,
				gasSurfaceAreaMultiplier = data.gasSurfaceAreaMultiplier,
				defaultMass = data.defaultMass,
				defaultTemperature = data.defaultTemperature,
				defaultPressure = data.defaultPressure,
				molarMass = data.molarMass,
				lightAbsorptionFactor = data.lightAbsorptionFactor,
				radiationAbsorptionFactor = data.radiationAbsorptionFactor,
				radiationPer1000Mass = data.radiationPer1000Mass,
				lowTempTransitionTarget = data.lowTempTransitionTarget,
				lowTemp = data.lowTemp,
				highTempTransitionTarget = data.highTempTransitionTarget,
				highTemp = data.highTemp,
				lowTempTransitionOreId = data.lowTempTransitionOreId,
				lowTempTransitionOreMassConversion = data.lowTempTransitionOreMassConversion,
				highTempTransitionOreId = data.highTempTransitionOreId,
				highTempTransitionOreMassConversion = data.highTempTransitionOreMassConversion,
				sublimateId = data.sublimateId,
				sublimateFx = data.sublimateFx,
				sublimateRate = data.sublimateRate,
				sublimateEfficiency = data.sublimateEfficiency,
				sublimateProbability = data.sublimateProbability,
				offGasPercentage = data.offGasPercentage,
				materialCategory = data.materialCategory,
				tags = data.tags,
				isDisabled = data.isDisabled,
				strength = data.strength,
				maxMass = data.maxMass,
				hardness = data.hardness,
				toxicity = data.toxicity,
				liquidCompression = data.liquidCompression,
				speed = data.speed,
				minHorizontalFlow = data.minHorizontalFlow,
				minVerticalFlow = data.minVerticalFlow,
				convertId = data.convertId,
				flow = data.flow,
				buildMenuSort = data.buildMenuSort,
				state = data.state,
				localizationID = data.localizationID,
				dlcId = dlc,
				composition = data.composition,
				description = data.description,
			};
		}
	}
}
