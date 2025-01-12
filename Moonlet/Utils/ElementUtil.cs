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
			if (SimHashNameLookup.ContainsKey(simHash))
			{
				Log.Warn("element already added!! " + name + " " + simHash);
			}
			SimHashNameLookup[simHash] = name;
			ReverseSimHashNameLookup[name] = simHash;

			return simHash;
		}

		public static List<SimHashes> GetLoadedElementsOnly(IEnumerable<string> elementIds)
		{
			if (elementIds == null)
				return null;

			if (ElementLoader.elements == null)
			{
				Log.Warn("Trying to check elements too early, Elements not initialized yet.");
				return null;
			}

			var result = new List<SimHashes>();

			foreach (var elementId in elementIds)
			{
				var element = ElementLoader.FindElementByName(elementId);
				if (element != null)
					result.Add(element.id);
			}

			return result;
		}

		public static SimHashes GetSimhashIfLoadedOrDefault(string elementId, SimHashes defaultSimhash)
		{
			var element = ElementLoader.FindElementByName(elementId);
			return element != null ? element.id : defaultSimhash;
		}

		public static bool TryGetSimhashIfLoaded(string elementId, out SimHashes simHashes)
		{
			simHashes = SimHashes.Void;

			var element = ElementLoader.FindElementByName(elementId);
			if (element != null)
			{
				simHashes = element.id;
				return true;
			}

			return false;
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

		public static Substance CreateSubstance(SimHashes id, Texture2D main, Texture2D spec, Texture2D normal, string anim, Element.State state, Color color, Material material, Color uiColor, Color conduitColor, Color? specularColor)
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
				if (main != null)
					newMaterial.SetTexture("_MainTex", main);

				if (spec != null)
					newMaterial.SetTexture("_ShineMask", spec);

				if (specularColor.HasValue)
					newMaterial.SetColor("_ShineColour", specularColor.Value);

				if (normal != null)
					newMaterial.SetTexture("_NormalNoise", normal);
			}

			var substance = ModUtil.CreateSubstance(stringId, state, animFile, newMaterial, color, uiColor, conduitColor);

			substance.anims = [substance.anim];

			return substance;
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
				if (File.Exists(mainPath))
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

			substance.anims = [substance.anim];

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

			if (reference == null)
			{
				Log.Warn("invalid audio config reference.");
				return null;
			}

			return new ElementsAudio.ElementAudioConfig()
			{
				elementID = id,
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
