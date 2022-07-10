using FUtility;
using HarmonyLib;
using System.Collections.Generic;
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
            private static Substance SetTexture(SimHashes elementId, bool shiny = false, Material reference = null)
            {
                var element = ElementLoader.FindElementByHash(elementId);
                var id = elementId.ToString().ToLower();

                var texture = FUtility.Assets.LoadTexture(id, texturePath);

                element.substance.material = new Material(reference ?? element.substance.material)
                {
                    mainTexture = texture
                };

                if (shiny)
                {
                    element.substance.material.SetTexture("_ShineMask", FUtility.Assets.LoadTexture(id + "_mask", texturePath)); // temporary);
                }

                return element.substance;
            }


            public static void Postfix(Dictionary<string, SubstanceTable> substanceTablesByDlc)
            {
                texturePath = Path.Combine(Utils.ModPath, "textures");
                var oreMaterial = ElementLoader.GetElement(SimHashes.Cuprite.CreateTag()).substance.material;
                var cobaltBlue = new Color32(0, 168, 255, 255);

                SetTexture(SimHashes.BrineIce); //, "frozen_brine_kanim");
                var cobalt = SetTexture(SimHashes.Cobalt, true, oreMaterial).material; //, "cobalt_refined_kanim");
                cobalt.SetColor("_ShineColour", cobaltBlue);
                SetTexture(SimHashes.Aerogel);//, "snow_kanim");
                SetTexture(SimHashes.RefinedCarbon, true, oreMaterial); //, "carbon_kanim");
                SetTexture(SimHashes.Creature); //, "carbon_kanim");
                SetTexture(SimHashes.CarbonFibre); //, "carbon_kanim");
                var bitumen = SetTexture(SimHashes.Bitumen).material; //, "carbon_kanim");
                bitumen.SetFloat("_WorldUVScale", 20f);

                // fix lead specular
                var lead = ElementLoader.FindElementByHash(SimHashes.Lead);
                lead.substance.material.SetTexture("_ShineMask", FUtility.Assets.LoadTexture("lead_mask_fixed", texturePath)); 
            }
        }
    }
}
