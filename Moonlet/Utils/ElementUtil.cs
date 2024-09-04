using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Moonlet.Utils
{
	public class ElementUtil
	{
		public static readonly Dictionary<SimHashes, string> SimHashNameLookup = [];
		public static readonly Dictionary<string, object> ReverseSimHashNameLookup = [];
		public static readonly List<ElementInfo> elements = [];

		public static SimHashes RegisterSimHash(string name)
		{
			var simHash = (SimHashes)Hash.SDBMLower(name);
			SimHashNameLookup.Add(simHash, name);
			ReverseSimHashNameLookup.Add(name, simHash);

			return simHash;
		}

		public static SimHashes GetSimhashSafe(string name, SimHashes defaultSimhash = SimHashes.Void)
		{
			if (name.IsNullOrWhiteSpace())
			{
				Log.Warn("Null SimHash value");
				return defaultSimhash;
			}
			try
			{
				return (SimHashes)Enum.Parse(typeof(SimHashes), name);
			}
			catch
			{
				if (int.TryParse(name, out var intValue))
					return (SimHashes)intValue;

				return (SimHashes)Hash.SDBMLower(name);
			}
		}

		public static Substance CreateSubstance(SimHashes id, bool specular, string assetsPath, string anim, Element.State state, Color color, Material material, Color uiColor, Color conduitColor, Color? specularColor, string normal)
		{
			var animFile = Assets.Anims.Find(a => a.name == anim)
				?? Assets.Anims.Find(a => a.name == "glass_kanim");

			var newMaterial = new Material(material);
			var stringId = SimHashNameLookup.TryGetValue(id, out var name) ? name : "";

			if (stringId.IsNullOrWhiteSpace())
			{
				Log.Warn("ID error");
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

		public static void SetTexture(Material material, string texturePath, string property)
		{
			if (FUtility.Assets.TryLoadTexture(texturePath, out var tex))
				material.SetTexture(property, tex);
			else
				Log.Warn("Could not load texture for " + texturePath);
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

		public static string[] GetAllWithTag(Tag tag)
		{
			return ElementLoader.elements
				.Where(e => e.HasTag(tag))
				.Select(e => e.id.ToString())
				.ToArray();
		}
	}
}
