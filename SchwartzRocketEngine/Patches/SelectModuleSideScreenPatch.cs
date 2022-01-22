using FUtility;
using HarmonyLib;

namespace SchwartzRocketEngine.Patches
{
    public class SelectModuleSideScreenPatch
    {
        //TODO: DLC only
        [HarmonyPatch(typeof(SelectModuleSideScreen), MethodType.Constructor)]
        public static class SelectModuleSideScreen_Ctor_Patch
        {
            public static void Postfix()
            {
                if (SelectModuleSideScreen.moduleButtonSortOrder.Contains(Buildings.SchwartzEngineClusterConfig.ID)) return;

               
            }
        }
    }
}
