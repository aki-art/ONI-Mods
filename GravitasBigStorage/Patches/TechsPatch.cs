using Database;
using GravitasBigStorage.Content;
using HarmonyLib;
using System.Collections.Generic;

namespace GravitasBigStorage.Patches
{
    public class TechsPatch
    {
        [HarmonyPatch(typeof(Techs), "Init")]
        public class Techs_TargetMethod_Patch
        {
            public static void Postfix(Techs __instance)
            {
                new Tech(GBSTechs.BIG_BOY_STORAGE, new List<string>
                {
                    GravitasBigStorageConfig.ID
                },
                __instance);
            }
        }
    }
}
