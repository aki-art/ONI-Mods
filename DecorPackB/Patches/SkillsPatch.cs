using HarmonyLib;

namespace DecorPackB.Patches
{
    public class SkillsPatch
    {
        [HarmonyPatch(typeof(Database.Skills), MethodType.Constructor, typeof(ResourceSet))]
        public class Database_Skills_Ctor_Patch
        {
            public static void Postfix(Database.Skills __instance)
            {
                ModDb.Skills.Register(__instance);
            }
        }
    }
}
