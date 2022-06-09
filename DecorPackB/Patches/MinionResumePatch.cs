using DecorPackB.Cmps;
using HarmonyLib;

namespace DecorPackB.Patches
{
    public class MinionResumePatch
    {
        [HarmonyPatch(typeof(MinionResume), "OnSpawn")]
        public class MinionResume_OnSpawn_Patch
        {
            public static void Prefix(MinionResume __instance)
            {
                if(__instance.TryGetComponent(out ArcheologistRestorer restorer))
                {
                    restorer.BeforeMinionResumeSpawn();
                }
            }
        }
    }
}
