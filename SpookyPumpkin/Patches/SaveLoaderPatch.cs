using HarmonyLib;
using SpookyPumpkinSO.Content.Cmps;
using System;

namespace SpookyPumpkinSO.Patches
{
    public class SaveLoaderPatch
    {
        [HarmonyPatch(typeof(SaveLoader), "Save", new Type[] { typeof(string), typeof(bool), typeof(bool) })]
        public class SaveLoader_Save_Patch
        {
            public static void Prefix()
            {
                foreach (SpiceRestorer restore in Mod.spiceRestorers)
                {
                    restore.OnSaveGame();
                }

                foreach (FacePaint facePaint in Mod.facePaints)
                {
                    facePaint.OnSaveGame();
                }
            }

            public static void Postfix()
            {
                foreach (SpiceRestorer restore in Mod.spiceRestorers)
                {
                    restore.OnLoadGame();
                }

                foreach (FacePaint facePaint in Mod.facePaints)
                {
                    facePaint.OnLoadGame();
                }
            }
        }
    }
}