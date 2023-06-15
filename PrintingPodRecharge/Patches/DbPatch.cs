using HarmonyLib;
using PrintingPodRecharge.Content;

namespace PrintingPodRecharge.Patches
{
	public class DbPatch
    {
        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Postfix(Db __instance)
            {
                ModDb.OnDbInit(__instance);

                if (Mod.otherMods.IsTwitchIntegrationHere)
					Integration.TwitchIntegration.TwitchMod.OnDbInit();
			}
        }
    }
}
