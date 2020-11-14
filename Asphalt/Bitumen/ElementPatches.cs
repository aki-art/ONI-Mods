using Harmony;
using UnityEngine;

namespace Asphalt.Bitumen
{
    public class ElementPatches
    {
        [HarmonyPatch(typeof(ElementLoader), "Load")]
        public static class Patch_ElementLoader_Load
        {
            public static void Postfix(SubstanceTable substanceTable)
            {
                Element bitumen = ElementLoader.FindElementByHash(SimHashes.Bitumen);
                ConfigureTags(bitumen);
                ConfigureSubstance(substanceTable.solidMaterial, bitumen);
            }

            private static void ConfigureTags(Element bitumen)
            {
                bitumen.materialCategory = GameTags.ManufacturedMaterial;
                bitumen.oreTags = new Tag[] {
                    GameTags.ManufacturedMaterial,
                    GameTags.BuildableAny,
                    GameTags.Solid
                };
            }

            private static void ConfigureSubstance(Material material, Element bitumen)
            {
                material.mainTexture = ModAssets.bitumenTexture;
                bitumen.substance = ModUtil.CreateSubstance(
                    name: "Bitumen",
                    state: Element.State.Solid,
                    kanim: Assets.Anims.Find(anim => anim.name == "solid_bitumen_kanim"), // too early to call Assets.GetAnim
                    material: material,
                    colour: Tuning.bitumenElementColor,
                    ui_colour: Tuning.bitumenElementColor,
                    conduit_colour: Tuning.bitumenElementColor
                    );
            }
        }
    }
}
