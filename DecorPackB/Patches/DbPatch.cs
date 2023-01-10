using DecorPackB.Integration.Twitch;
using HarmonyLib;

namespace DecorPackB.Patches
{
    public class DbPatch
    {
        [HarmonyPatch(typeof(Db), "Initialize")]
        public class Db_Initialize_Patch
        {
            public static void Postfix()
            {
                ModAssets.PostDbInit();
                TwitchEvents.PostDbInit();
            }
        }
    }
}
