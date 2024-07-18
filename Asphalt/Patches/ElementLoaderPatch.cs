using FUtility;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Asphalt.Patches
{
	// Prepare Bitumen to be introduced to actual gameplay

	// Reenable bitumen, just in case someone disabled it
	public class ElementLoaderPatch
	{
		[HarmonyPatch(typeof(ElementLoader), nameof(ElementLoader.CollectElementsFromYAML))]
		[HarmonyPriority(Priority.Last)]
		public class ElementLoader_CollectElementsFromYAML_Patch
		{
			public static void Postfix(ref List<ElementLoader.ElementEntry> __result)
			{
				var bitumenId = SimHashes.Bitumen.ToString();
				foreach (var elem in __result)
				{
					if (elem.elementId == bitumenId)
					{
						elem.isDisabled = false;
						return;
					}
				}
			}
		}

		// set appropiate tags and textures
		[HarmonyPatch(typeof(ElementLoader), nameof(ElementLoader.Load))]
		[HarmonyPriority(Priority.Low)]
		public static class Patch_ElementLoader_Load
		{
			private static void AddTag(ref Tag[] tags, Tag tag)
			{
				// dont add duplicate tags (in case another mod added stuff before me)
				foreach (Tag t in tags)
					if (t == tag) return;

				tags = tags.AddToArray(tag);
			}

			private static void RemoveTags(ref Tag[] oreTags, HashSet<Tag> removeThese)
			{
				var newTags = new List<Tag>();

				foreach (var tag in oreTags)
				{
					if (!removeThese.Contains(tag))
						newTags.Add(tag);
				}

				oreTags = newTags.ToArray();
			}

			private static Tag CreateMaterialCategoryTag(Tag phaseTag, string materialCategoryField)
			{
				return string.IsNullOrEmpty(materialCategoryField)
					? phaseTag
					: TagManager.Create(materialCategoryField);
			}

			public static void Postfix(Dictionary<string, SubstanceTable> substanceTablesByDlc)
			{
				Element bitumen = ElementLoader.FindElementByHash(SimHashes.Bitumen);
				//bitumen.materialCategory = GameTags.ManufacturedMaterial;
				bitumen.materialCategory = CreateMaterialCategoryTag(TagManager.Create("Solid"), GameTags.ManufacturedMaterial.ToString()); // This tag is for storage

				if (bitumen.oreTags is null)
					bitumen.oreTags = new Tag[] { };

				AddTag(ref bitumen.oreTags, GameTags.ManufacturedMaterial);
				AddTag(ref bitumen.oreTags, GameTags.BuildableAny);
				AddTag(ref bitumen.oreTags, GameTags.Solid);
				AddTag(ref bitumen.oreTags, ModTags.RoadSurfaceMaterial);
				RemoveTags(ref bitumen.oreTags, new HashSet<Tag>()
				{
					GameTags.HideFromCodex,
					GameTags.HideFromSpawnTool
				});

				Log.Debug("Bitumen tags:");
				foreach (var tag in bitumen.oreTags)
					Log.Debug($"- {tag}");
				if (bitumen.substance is null)
				{
					Log.Warning("Bitumen has no substance.");
					return;
				}

				bitumen.substance = ModUtil.CreateSubstance(
					"bitumen",
					Element.State.Solid,
					Assets.Anims.Find(anim => anim.name == "solid_bitumen_kanim"),
					new Material(bitumen.substance.material),
					ModAssets.Colors.bitumen,
					ModAssets.Colors.bitumen,
					ModAssets.Colors.bitumen);

				bitumen.substance.material.mainTexture = ModAssets.bitumenTexture;
			}

		}
	}
}
