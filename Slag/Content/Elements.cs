using FUtility;
using Slag.Patches;
using System.Collections;
using System.IO;
using UnityEngine;

namespace Slag.Content
{
    public class Elements
    {
        public static SimHashes Slag = EnumPatch.RegisterSimHash("Slag");
        public static SimHashes SlagGlass = EnumPatch.RegisterSimHash("SlagGlass");
        public static SimHashes MoltenSlagGlass = EnumPatch.RegisterSimHash("MoltenSlagGlass");

        public static void RegisterSubstances(Hashtable substanceList)
        {
            substanceList.Add(Slag, CreateSubstance(Slag, "slag_kanim", Element.State.Solid, ModAssets.Colors.slag));
            substanceList.Add(SlagGlass, CreateSubstance(SlagGlass, "glass_kanim", Element.State.Solid, ModAssets.Colors.slagGlass));
            substanceList.Add(MoltenSlagGlass, CreateSubstance(MoltenSlagGlass, "liquid_tank_kanim", Element.State.Liquid, ModAssets.Colors.moltenSlagGlass));
        }

        public static Substance CreateSubstance(SimHashes id, string uiAnim, Element.State state, Color color)
        {
            var animFile = Assets.Anims.Find(anim => anim.name == uiAnim);
            var material = GetMaterialForState(state);

            return ModUtil.CreateSubstance(id.ToString(), state, animFile, material, color, color, color);
        }

        private static Material GetMaterialForState(Element.State state)
        {
            // (gases use liquid material)
            var material = state == Element.State.Solid ? Assets.instance.substanceTable.solidMaterial : Assets.instance.substanceTable.liquidMaterial;
            return new Material(material);
        }

        public static void SetSolidMaterials()
        {
            var folder = Path.Combine(Utils.ModPath, "assets", "elements", "textures");
            //var shinyMaterial = Assets.instance.substanceTable.GetSubstance(SimHashes.Diamond).material;
            var shinyMaterial = Assets.instance.substanceTable.GetSubstance(SimHashes.Cuprite).material;

            SetTextures(Slag, null, folder, "slag");
            SetTextures(SlagGlass, shinyMaterial, folder, "slag_glass", "slag_glass_specular");
        }

        public static Material SetTextures(SimHashes id, Material newMaterial, string folder, string texture, string spec = null)
        {
            var substance = ElementLoader.FindElementByHash(id).substance;
            var tex = FUtility.Assets.LoadTexture(texture, folder);

            if (newMaterial != null)
            {
                substance.material = new Material(newMaterial);
            }

            substance.material.mainTexture = tex;

            if (!spec.IsNullOrWhiteSpace())
            {
                var specTex = FUtility.Assets.LoadTexture(spec, folder);
                substance.material.SetTexture("_ShineMask", specTex);
            }

            return substance.material;
        }
    }
}
