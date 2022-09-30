using HarmonyLib;
using PrintingPodRecharge.Cmps;
using System;

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
                    if (identity.TryGetComponent(out CustomDupe hairDye))
                    {
                        hairDye.OnLoadGame();
                    }
                }
            }
        }
    }
}
