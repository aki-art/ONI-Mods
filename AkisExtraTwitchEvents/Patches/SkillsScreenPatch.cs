using HarmonyLib;

namespace Twitchery.Patches
{
	public class SkillsScreenPatch
	{
        [HarmonyPatch(typeof(SkillsScreen), "SortRows")]
        public class SkillsScreen_SortRows_Patch
        {
            public static void Prefix(SkillsScreen __instance)
            {
                for(int i = __instance.sortableRows.Count - 1; i >= 0; i--)
                {
                    var ai = __instance.sortableRows[i].assignableIdentity;
                    if(__instance.sortableRows.FindAll(r => r.assignableIdentity == ai).Count >= 2)
                    {
                        __instance.sortableRows.RemoveAt(i);
                        Util.KDestroyGameObject(ai.GetSoleOwner().gameObject);
                    }
                }
            }
        }
	}
}
