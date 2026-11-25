using System;
using System.Collections.Generic;
using System.Text;
using TMPro;

namespace SPFrightfulLands
{
    public class ModAssets
    {
        public const string PURPLE_RED_GRADIENT = "SPFLRedPurpleGradient";

        public static void LoadAll()
        {
            TmpExtensions();
        }

        private static void TmpExtensions()
        {
            var purple = Util.ColorFromHex("911b79");
            var red = Util.ColorFromHex("ce0411");

            MaterialReferenceManager.AddColorGradientPreset(
                TMP_TextUtilities.GetSimpleHashCode(PURPLE_RED_GRADIENT),
                new TMP_ColorGradient(
                    purple, purple,
                    red, red));

            Log.Debug($"registered color gradient {Utils.StringToTMPHash(PURPLE_RED_GRADIENT)} {TMP_TextUtilities.GetSimpleHashCode(PURPLE_RED_GRADIENT)}");

            if (MaterialReferenceManager.TryGetColorGradientPreset(TMP_TextUtilities.GetSimpleHashCode(PURPLE_RED_GRADIENT), out var gradient))
                Log.Debug($"has gradient {gradient.bottomLeft}");
            else
                Log.Debug("no gradient");
        }

    }
}
