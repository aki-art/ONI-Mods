using CrittersDropBones.Content;
using HarmonyLib;

namespace CrittersDropBones.Patches
{
    public class DbPatch
    {
        [HarmonyPatch(typeof(Db), "Initialize")]
        public class Db_Initialize_Patch
        {
            [HarmonyPriority(Priority.High)] // so mods altering recipes can run after and modify these
            public static void Postfix()
            {
                CDBRecipes.AddAllRecipes();
            }
        }
    }
}
