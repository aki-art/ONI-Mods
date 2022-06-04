using FUtility;
using System.Collections;
using System.IO;
using UnityEngine;

namespace TransparentAluminum.Content
{
    public class Elements : ElementsBase
    {
        public static readonly SimHashes TransparentAluminum = RegisterSimHash("TransparentAluminum");

        public static void RegisterSubstances(Hashtable substanceList)
        {
            substanceList.Add(TransparentAluminum, CreateSubstance(TransparentAluminum, "glass_kanim", Element.State.Solid, Color.cyan));
        }

        public static void SetSolidMaterials()
        {
            var folder = Path.Combine(Utils.ModPath, "assets", "textures");

            var oreMaterial = Assets.instance.substanceTable.GetSubstance(SimHashes.Cuprite).material;
            SetTextures(TransparentAluminum, oreMaterial, folder, "transparent_aluminum"); //, "transparent_aluminum_specular");
        }
    }
}
