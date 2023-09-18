using FUtility;
using HarmonyLib;
using System.IO;
using UnityEngine;

namespace CompleteElementTextures.Patches
{
    // Prepare Bitumen to be introduced to actual gameplay
    public class ElementLoaderPatch
    {
        private static string texturePath;

        [HarmonyPatch(typeof(ElementLoader), "Load")]
        public static class Patch_ElementLoader_Load
        {
            public static void Postfix()
            {
                texturePath = Path.Combine(Utils.ModPath, "textures");
                var metalMaterial = ElementLoader.GetElement(SimHashes.Steel.CreateTag()).substance.material;
                var cobaltBlue = new Color32(0, 168, 255, 255);

                SetTexture(SimHashes.BrineIce, "brineice_kanim");
                SetTexture(SimHashes.Aerogel, "aerogel_kanim");
                SetTexture(SimHashes.RefinedCarbon, "refinedcarbon_kanim", true, metalMaterial);
                SetTexture(SimHashes.Creature, "creature_kanim");
                SetTexture(SimHashes.CarbonFibre, "carbonfibre_kanim");
                SetTexture(SimHashes.Bitumen, "bitumen_kanim");

                // fix lead specular
                var lead = ElementLoader.FindElementByHash(SimHashes.Lead);
                lead.substance.material.SetTexture("_ShineMask", FUtility.FAssets.LoadTexture("lead_mask_fixed", texturePath));

                // DLC
                if (DlcManager.IsExpansion1Active())
                {
                    var cobaltMaterial = SetTexture(SimHashes.Cobalt, null, true, metalMaterial).material;
                    cobaltMaterial.SetColor("_ShineColour", cobaltBlue);
                }
            }

            private static Substance SetTexture(SimHashes elementId, string anim = null, bool shiny = false, Material reference = null)
            {
                var element = ElementLoader.FindElementByHash(elementId);

                if(element == null)
                {
                    return null;
                }

                var id = elementId.ToString().ToLower();

                var texture = FUtility.FAssets.LoadTexture(id, texturePath);

                element.substance.material = new Material(reference ?? element.substance.material)
                {
                    mainTexture = texture
                };

                if (shiny)
                {
                    element.substance.material.SetTexture("_ShineMask", FUtility.FAssets.LoadTexture(id + "_mask", texturePath)); // temporary);
                }

                if (anim != null)
                {
                    element.substance.anim = Assets.GetAnim(anim);
                }

                return element.substance;
            }
        }
    }
}
