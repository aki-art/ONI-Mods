﻿using FUtility.FLocalization;
using HarmonyLib;

namespace PrintingPodRecharge.Patches
{
	public class LocalizationPatch
    {
        [HarmonyPatch(typeof(Localization), "Initialize")]
        public class Localization_Initialize_Patch
        {
            public static void Postfix()
            {
                Translations.RegisterForTranslation(typeof(STRINGS), true);
            }
        }
    }
}
