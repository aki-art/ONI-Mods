/*using HarmonyLib;
using Twitchery.Content.Scripts;

namespace Twitchery.Patches
{
    public class PlanScreenPatch
    {

        [HarmonyPatch(typeof(PlanScreen), "Refresh")]
        public class PlanScreen_Refresh_Patch
        {
            public static void Postfix()
            {
                if(AETEScreenPipmanager.Instance != null)
                {
                    AETEScreenPipmanager.Instance.Refresh();
                }
            }
        }
    }
}
*/