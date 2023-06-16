using HarmonyLib;
namespace Twitchery.Content.Scripts
{
    public class WaterCubesPatch
    {
        [HarmonyPatch(typeof(WaterCubes), "Init")]
        public class WaterCubes_Init_Patch
        {
            [HarmonyPriority(Priority.LowerThanNormal)]
            public static void Postfix(WaterCubes __instance)
            {
                if(AkisTwitchEvents.Instance == null)
                {
                    return;
                }

                AkisTwitchEvents.Instance.ApplyLiquidTransparency(__instance);
            }
        }
    }
}
