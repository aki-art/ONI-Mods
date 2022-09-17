using HarmonyLib;
using PrintingPodRecharge.Cmps;

namespace PrintingPodRecharge.Patches
{
    public class MinionIdentityPatch
    {
        [HarmonyPatch(typeof(MinionIdentity), "OnSpawn")]
        public class MinionIdentity_OnSpawn_Patch
        {
            public static void Prefix(MinionIdentity __instance)
            {
                if (__instance.TryGetComponent(out HairDye dye) && dye.dyedHair)
                {
                    dye.OnLoadGame();
                }
            }
        }
    }
}
