using FUtility;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Asphalt.Patches
{
    // Prepare Bitumen to be introduced to actual gameplay
    public class ElementLoaderPatch
    {
        [HarmonyPatch(typeof(ElementLoader), "Load")]
        public static class Patch_ElementLoader_Load
        {
            private static void AddTag(ref Tag[] tags, Tag tag)
            {
                // dont add duplicate tags (in case another mod added stuff before me)
                foreach (Tag t in tags)
                {
                    if (t == tag) return;
                }

                tags = tags.AddToArray(tag);
            }
            private static Tag CreateMaterialCategoryTag(Tag phaseTag, string materialCategoryField)
            {
                if (string.IsNullOrEmpty(materialCategoryField))
                    return phaseTag;

                return TagManager.Create(materialCategoryField);
            }

            public static void Postfix(Dictionary<string, SubstanceTable> substanceTablesByDlc)
            {
                Element bitumen = ElementLoader.FindElementByHash(SimHashes.Bitumen);
                //bitumen.materialCategory = GameTags.ManufacturedMaterial;
                bitumen.materialCategory = CreateMaterialCategoryTag(TagManager.Create("Solid"), GameTags.ManufacturedMaterial.ToString()); // This tag is for storage

                if (bitumen.oreTags is null)
                {
                    bitumen.oreTags = new Tag[] { };
                }

                AddTag(ref bitumen.oreTags, GameTags.ManufacturedMaterial);
                AddTag(ref bitumen.oreTags, GameTags.BuildableAny);
                AddTag(ref bitumen.oreTags, GameTags.Solid);

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
                    ModAssets.Colors.bitumen );

                bitumen.substance.material.mainTexture = ModAssets.bitumenTexture;
            }
        }
    }
}
