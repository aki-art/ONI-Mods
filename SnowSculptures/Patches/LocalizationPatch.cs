﻿using FUtility;
using HarmonyLib;

namespace SnowSculptures.Patches
{
    public class LocalizationPatch
    {
        [HarmonyPatch(typeof(Localization), "Initialize")]
        public class Localization_Initialize_Patch
        {
            public static void Postfix()
            {
                Loc.Translate(typeof(STRINGS), true);

                Strings.Add("STRINGS.PREFABS.BUILDINGS.SNOWSCULPTURES_SNOWSCULPTURE.DESC", global::STRINGS.BUILDINGS.PREFABS.MARBLESCULPTURE.DESC);
                Strings.Add("STRINGS.PREFABS.BUILDINGS.SNOWSCULPTURES_SNOWSCULPTURE.EFFECT", global::STRINGS.BUILDINGS.PREFABS.MARBLESCULPTURE.EFFECT);
            }
        }
    }
}
