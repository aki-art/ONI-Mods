using Bestagons.Content.ModDb;
using HarmonyLib;
namespace Bestagons.Content.Scripts
{
    public class DbPatch
    {

        [HarmonyPatch(typeof(Db), "Initialize")]
        public class Db_Initialize_Patch
        {
            public static void Postfix()
            {
                BDb.Initialize();
            }
        }
    }
}
