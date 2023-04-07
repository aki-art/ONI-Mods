using GravitasBigStorage.Content;
using HarmonyLib;

namespace GravitasBigStorage.Patches
{
    public class DbPatch
    {
        [HarmonyPatch(typeof(Db), "Initialize")]
        public class Db_Initialize_Patch
        {
            public static void Postfix()
            {
                ModAssets.LoadAll();
                GBSStatusItems.Register();

#if DEBUG
                Db.Get().Quests.LonelyMinionPowerQuest.Criteria[0].TargetValues[0] = 10f;
#endif
            }
        }
    }
}
