using Database;
using HarmonyLib;
using Slag.Content.Buildings;
using System.Collections.Generic;

namespace Slag.Patches
{
    public class TechsPatch
    {
        // add tech to database
        [HarmonyPatch(typeof(Techs), "Init")]
        public class Techs_TargetMethod_Patch
        {
            public static void Postfix(Techs __instance)
            {
                new Tech(ModAssets.Techs.ADVANCED_INSULATION_ID, new List<string>
                {
                    InsulatedWindowTileConfig.ID,
                    InsulatedPressureDoorConfig.ID,
                    InsulatedManualPressureDoorConfig.ID
                },
                __instance);
            }
        }
    }
}
