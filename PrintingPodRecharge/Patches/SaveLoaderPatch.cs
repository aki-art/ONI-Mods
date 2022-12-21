using FUtility;
using HarmonyLib;
using KSerialization;
using PrintingPodRecharge.Content;
using PrintingPodRecharge.Content.Cmps;
using System;
using UnityEngine;

namespace PrintingPodRecharge.Patches
{
    public class SaveLoaderPatch
    {
        [HarmonyPatch(typeof(SaveLoader), "Save", new Type[] { typeof(string), typeof(bool), typeof(bool) })]
        public class SaveLoader_Save_Patch
        {
            public static void Prefix()
            {
                foreach (MinionIdentity identity in Components.MinionIdentities)
                {
                    if (identity.TryGetComponent(out CustomDupe hairDye))
                    {
                        hairDye.OnSaveGame();
                    }
                }
            }

            public static void Postfix()
            {
                foreach (MinionIdentity identity in Components.MinionIdentities)
                {
                    if (identity.TryGetComponent(out CustomDupe customDupe))
                    {
                        customDupe.OnLoadGame();
                    }
                }
            }
        }
    }
}
