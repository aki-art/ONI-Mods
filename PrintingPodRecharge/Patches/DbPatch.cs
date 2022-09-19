using HarmonyLib;
using TUNING;

namespace PrintingPodRecharge.Patches
{
    public class DbPatch
    {
        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                ModAssets.LateLoadAssets();
            }

            public static void Postfix()
            {
                // gene shuffler traits were marked as negative for some reason. Possibly an oversight.
                foreach (var trait in DUPLICANTSTATS.GENESHUFFLERTRAITS)
                {
                    Db.Get().traits.Get(trait.id).PositiveTrait = true;
                }
            }
        }
    }
}
