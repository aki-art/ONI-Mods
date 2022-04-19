using HarmonyLib;
using PrintingPodRecharge.Cmps;
using TUNING;

namespace PrintingPodRecharge.Patches
{
    public class MinionStartingStatsPatch
    {
        [HarmonyPatch(typeof(MinionStartingStats), "GenerateTraits")]
        public class MinionStartingStats_GenerateTraits_Patch
        {
            public static void Prefix(MinionStartingStats __instance)
            {
                if (ImmigrationModifier.Instance != null && ImmigrationModifier.Instance.IsBundleSuperDuplicant())
                {
                    var trait = Db.Get().traits.Get(DUPLICANTSTATS.GENESHUFFLERTRAITS.GetRandom().id);
                    __instance.Traits.Add(trait);
                }
            }

            public static void Postfix(ref int __result)
            {
                if (ImmigrationModifier.Instance != null && ImmigrationModifier.Instance.IsBundleSuperDuplicant())
                {
                    __result += 8;
                }
            }
        }
    }
}
