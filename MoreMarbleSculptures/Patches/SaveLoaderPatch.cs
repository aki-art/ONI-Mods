﻿using FUtilityArt.Components;
using HarmonyLib;
using System;

namespace MoreMarbleSculptures.Patches
{
    public class SaveLoaderPatch
    {
        [HarmonyPatch(typeof(SaveLoader), "Save", new Type[] { typeof(string), typeof(bool), typeof(bool) })]
        public class SaveLoader_Save_Patch
        {
            public static void Prefix()
            {
                foreach(ArtOverrideRestorer restore in Mod.artRestorers)
                {
                    restore.OnSaveGame();
                }
            }

            public static void Postfix()
            {
                foreach (ArtOverrideRestorer restore in Mod.artRestorers)
                {
                    restore.Restore();
                }
            }
        }
    }
}