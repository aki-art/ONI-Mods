using HarmonyLib;
using PrintingPodRecharge.Cmps;

namespace PrintingPodRecharge.Patches
{
    public class AccessorizerPatch
    {

        [HarmonyPatch(typeof(Accessorizer), "OnSpawn")]
        public class Accessorizer_OnSpawn_Patch
        {
            public static void Prefix(Accessorizer __instance)
            {
                if(__instance.TryGetComponent(out CustomDupe customDupe))
                {
                    __instance.GetComponent<MinionIdentity>().personalityId = Db.Get().Personalities.GetPersonalityFromNameStringKey("MEEP").IdHash;
                }
            }
        }
    }
}
