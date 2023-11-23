using HarmonyLib;
using Klei.AI;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace FUtility.ElementUtil
{
	public class ElementUtil
	{
		public static readonly Dictionary<SimHashes, string> SimHashNameLookup = new Dictionary<SimHashes, string>();
		public static readonly Dictionary<string, object> ReverseSimHashNameLookup = new Dictionary<string, object>();
		public static readonly List<ElementInfo> elements = new List<ElementInfo>();

		public static SimHashes RegisterSimHash(string name)
		{
			var simHash = (SimHashes)Hash.SDBMLower(name);
			SimHashNameLookup.Add(simHash, name);
			ReverseSimHashNameLookup.Add(name, simHash);

			return simHash;
		}

		public static Substance CreateSubstance(SimHashes id, bool specular, string anim, Element.State state, Color color, Material material, Color uiColor, Color conduitColor, Color? specularColor, string normal)
		{
			var animFile = global::Assets.Anims.Find(a => a.name == anim);

			if (animFile == null)
			{
				animFile = global::Assets.Anims.Find(a => a.name == "glass_kanim");
			}

			var newMaterial = new Material(material);

			Log.Assert("newMaterial", newMaterial);

			if (state == Element.State.Solid)
			{
				SetTexture(newMaterial, id.ToString().ToLowerInvariant(), "_MainTex");

				if (specular)
				{
					SetTexture(newMaterial, id.ToString().ToLowerInvariant() + "_spec", "_ShineMask");

					if (specularColor.HasValue)
					{
						newMaterial.SetColor("_ShineColour", specularColor.Value);
					}
				}

				if (!normal.IsNullOrWhiteSpace())
				{
					SetTexture(newMaterial, normal, "_NormalNoise");
				}
			}

			var substance = ModUtil.CreateSubstance(id.ToString(), state, animFile, newMaterial, color, uiColor, conduitColor);

			Log.Debug("created substance");
			Log.Assert("substance", substance);

			return substance;
		}

		// TODO: load from an assetbundle later
		private static void SetTexture(Material material, string texture, string property)
		{
			var path = Path.Combine(Utils.ModPath, "assets", "textures", texture + ".png");

			if (FAssets.TryLoadTexture(path, out var tex))
			{
				material.SetTexture(property, tex);
			}
			else Debug.Log("no texture " + path);
		}

		public static void AddModifier(Element element, float decor, float overHeat)
		{
			if (decor != 0)
			{
				element.attributeModifiers.Add(new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, decor, element.name, true));
			}

			if (overHeat != 0)
			{
				element.attributeModifiers.Add(new AttributeModifier(Db.Get().BuildingAttributes.OverheatTemperature.Id, overHeat, element.name, false));
			}
		}

		// The game incorrectly assigns the display name to elements not in the original SimHashes table,
		// so this needs to be changed to the actual ID. 
		// Fixed in 2023 october QoL
		public static void FixTags()
		{
			var fieldInfo = AccessTools.Field(typeof(Substance), "nameTag");

			if(fieldInfo != null)
			{
				foreach (var elem in elements)
				{
					fieldInfo.SetValue(elem.Get().substance, TagManager.Create(elem.SimHash.ToString()));
				}
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
	}
}
