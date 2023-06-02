using HarmonyLib;
using MoreMarbleSculptures.FUtilityArt.Components;

namespace MoreMarbleSculptures.Patches
{
    public class SaveLoaderPatch
    {
        [HarmonyPatch(typeof(SaveLoader), "Save", new[] 
        { 
            typeof(string), 
            typeof(bool), 
            typeof(bool) 
        })]
        public class SaveLoader_Save_Patch
        {
            public static void Prefix()
            {
                foreach (ArtOverrideRestorer restore in Mod.artRestorers)
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