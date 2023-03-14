using DecorPackA.Buildings;
using HarmonyLib;
using System;

namespace DecorPackA.Patches
{
    public class SaveLoaderPatch
    {
        // prevent the game trying to remember and load a non-existing facade after the mod has been uninstalled
        [HarmonyPatch(typeof(SaveLoader), "Save", new Type[] { typeof(string), typeof(bool), typeof(bool) })]
        public class SaveLoader_Save_Patch
        {
            public static void Prefix(ref string __state)
            {
                if(ModDb.myFacades.Contains(PlanScreen.Instance.lastSelectedBuildingFacade))
                {
                    __state = PlanScreen.Instance.lastSelectedBuildingFacade;
                    PlanScreen.Instance.lastSelectedBuildingFacade = "DEFAULT_FACADE";
                }

                foreach(FacadeRestorer restorer in Mod.facadeRestorers)
                {
                    restorer.OnSave();
                }
            }

            public static void Postfix(ref string __state)
            {
                if(!__state.IsNullOrWhiteSpace())
                {
                    Log.Debuglog("restoring facade: " + __state);
                    PlanScreen.Instance.lastSelectedBuildingFacade = __state;
                }

                foreach (FacadeRestorer restorer in Mod.facadeRestorers)
                {
                    restorer.AfterSave();
                }
            }
        }
    }
}
