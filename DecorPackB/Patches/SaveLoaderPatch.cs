using DecorPackB.Cmps;
using FUtility.Components;
using HarmonyLib;
using System;

namespace DecorPackB.Patches
{
    public class SaveLoaderPatch
    {
        [HarmonyPatch(typeof(SaveLoader), "Save", new Type[] { typeof(string), typeof(bool), typeof(bool) })]
        public class SaveLoader_Save_Patch
        {
            public static void Prefix()
            {
                foreach(Restorer restore in Mod.restorers)
                {
                    restore.OnSaveGame();
                }
            }

            public static void Postfix()
            {
                foreach (Restorer restore in Mod.restorers)
                {
                    restore.OnRestore();
                }
            }
        }
    }
}
