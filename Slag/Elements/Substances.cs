using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Slag.Elements
{
    public class Substances
    {
        private static readonly string assetDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "assets");
        public static Substance SlagGlassElement(Material solidMaterial)
        {
            Substance slagGlasssubstance = null;
            KAnimFile animFile = Assets.Anims.Find(anim => anim.name == "glass_kanim");

            var slagGlassSubstanceTexture = ModAssets.LoadTexture("slag_glass", assetDirectory);
            var slagGlassShineTexture = ModAssets.LoadTexture("slag_glass_mask", assetDirectory);
            var slagGlassNormalMap = ModAssets.LoadTexture("normal_slag_glass", assetDirectory);

            if (slagGlassSubstanceTexture == null || slagGlassShineTexture == null || slagGlassNormalMap == null)
            {
                Log.Warning("Texture files not found for Slag Glass.");
                return null;
            }

            Material material = UnityEngine.Object.Instantiate(solidMaterial);
            material.mainTexture = slagGlassSubstanceTexture;
            material.SetTexture("_ShineMask", slagGlassShineTexture);
            material.SetTexture("_NormalNoise", slagGlassNormalMap);

            var slagGlassElementColor = Color.cyan;

            try
            {
                slagGlasssubstance = ModUtil.CreateSubstance(
                    name: "SlagGlass",
                    state: Element.State.Solid,
                    kanim: animFile,
                    material: material,
                    colour: slagGlassElementColor,
                    ui_colour: slagGlassElementColor,
                    conduit_colour: slagGlassElementColor
                    );
            }
            catch (Exception e)
            {
                Log.Error("Could not assign new material to Slag Glass element: " + e);
            }

            return slagGlasssubstance;
        }

        public static Substance SlagElement(Material solidMaterial)
        {
            Substance slagsubstance = null;
            KAnimFile animFile = Assets.Anims.Find(anim => anim.name == "solid_bitumen_kanim");

            var slagSubstanceTexture = ModAssets.LoadTexture("slag", assetDirectory);

            if (slagSubstanceTexture == null)
            {
                Log.Warning("Texture file not found for Slag.");
                return null;
            }

            Material material = UnityEngine.Object.Instantiate(solidMaterial);
            material.mainTexture = slagSubstanceTexture;
            var slagElementColor = Color.gray;

            try
            {
                slagsubstance = ModUtil.CreateSubstance(
                    name: "Slag",
                    state: Element.State.Solid,
                    kanim: animFile,
                    material: material,
                    colour: slagElementColor,
                    ui_colour: slagElementColor,
                    conduit_colour: slagElementColor
                    );
            }
            catch (Exception e)
            {
                Log.Error("Could not assign new material to Slag element: " + e);
            }

            return slagsubstance;
        }
    }
}
