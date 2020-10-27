using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using FUtility;

namespace TransparentAluminium
{
    class TransparentAluminum
    {
        public static Substance SlagGlassElement(Material solidMaterial)
        {
            Substance substance = null;
            KAnimFile animFile = Assets.Anims.Find(anim => anim.name == "glass_kanim");
            //KAnimFile animFile = Assets.GetAnim("glass_kanim");

            var text = ModAssets.GetTexture("slag_glass");
            var spec = ModAssets.GetTexture("slag_glass_mask");
            var normal = ModAssets.GetTexture("normal_slag_glass");

            if (text == null || spec == null || normal == null)
            {
                Log.Warning("Texture files not found for Transparent Aluminum.");
                return null;
            }

            Material material = UnityEngine.Object.Instantiate(solidMaterial);
            material.mainTexture = text;
            material.SetTexture("_ShineMask", spec);
            material.SetTexture("_NormalNoise", normal);

            var elementColor = Color.cyan;

            substance = ModUtil.CreateSubstance(
                   name: "TransparentAluminum",
                   state: Element.State.Solid,
                   kanim: animFile,
                   material: material,
                   colour: elementColor,
                   ui_colour: elementColor,
                   conduit_colour: elementColor
                   );

            return substance;
        }

    }
}
