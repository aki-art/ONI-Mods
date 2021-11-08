using FUtility;
using HarmonyLib;
using System.Collections.Generic;

namespace Asphalt.Patches
{
    // Prepare Bitumen to be introduced to actual gameplay
    class ElementLoaderPatch
    {
        [HarmonyPatch(typeof(ElementLoader), "Load")]
        public static class Patch_ElementLoader_Load
        {
            public static void Postfix(Dictionary<string, SubstanceTable> substanceTablesByDlc)
            {
                Element bitumen = ElementLoader.FindElementByHash(SimHashes.Bitumen);
                bitumen.materialCategory = GameTags.ManufacturedMaterial;

                if (bitumen.oreTags is null)
                {
                    bitumen.oreTags = new Tag[] { };
                }

                bitumen.oreTags.AddToArray(GameTags.ManufacturedMaterial);
                bitumen.oreTags.AddToArray(GameTags.BuildableAny);
                bitumen.oreTags.AddToArray(GameTags.Solid);

                if(bitumen.substance is null)
                {
                    Log.Warning("Bitumen has no substance.");
                    return;
                } 

                bitumen.substance.material.mainTexture = ModAssets.bitumenTexture;
                bitumen.substance.anim = Assets.Anims.Find(anim => anim.name == "solid_bitumen_kanim");
                bitumen.substance.colour = ModAssets.Colors.bitumen;
                bitumen.substance.conduitColour = ModAssets.Colors.bitumen;
                bitumen.substance.uiColour = ModAssets.Colors.bitumen;
            }
        }
    }
}
