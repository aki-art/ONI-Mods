using GravitasBigStorage.Content;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GravitasBigStorage.Patches
{
    internal class DbPatch
    {

        [HarmonyPatch(typeof(Db), "Initialize")]
        public class Db_Initialize_Patch
        {
            public static void Postfix(Db __instance)
            {
                GBSStatusItems.Register();

#if DEBUG
                __instance.Quests.LonelyMinionPowerQuest.Criteria[0].TargetValues[0] = 10f;
#endif
            }
        }
    }
}
