﻿using GravitasBigStorage.Content;
using HarmonyLib;

namespace GravitasBigStorage.Patches
{
    internal class DetailsScreenPatch
    {
        [HarmonyPatch(typeof(DetailsScreen), "OnPrefabInit")]
        public static class DetailsScreen_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                FUtility.FUI.SideScreen.AddClonedSideScreen<AnalyzableSideScreen>(
                    "Analyzable Side Screen",
                    typeof(ButtonMenuSideScreen));
            }
        }
    }
}
