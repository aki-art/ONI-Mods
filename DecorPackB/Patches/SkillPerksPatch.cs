using HarmonyLib;

namespace DecorPackB.Patches
{
    public class SkillPerksPatch
    {
        [HarmonyPatch(typeof(Database.SkillPerks), MethodType.Constructor, typeof(ResourceSet))]
        public class Database_SkillPerks_Ctor_Patch
        {
            public static void Postfix(Database.SkillPerks __instance)
            {
                ModAssets.SkillPerks.Register(__instance);
            }
        }
    }
}
