using HarmonyLib;
using PrintingPodRecharge.Content;
using System.Linq;
using TUNING;

namespace PrintingPodRecharge.Patches
{
    public class DbPatch
    {
        public static Personality Meep;

        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
            }

            public static void Postfix(Db __instance)
            {
                Meep = Db.Get().Personalities.resources.Find(p => p.nameStringKey == "MEEP");

                // gene shuffler traits were marked as negative for some reason. Possibly an oversight.
                foreach (var trait in DUPLICANTSTATS.GENESHUFFLERTRAITS)
                {
                    Db.Get().traits.Get(trait.id).PositiveTrait = true;
                }

                ModAssets.goodTraits = DUPLICANTSTATS.GOODTRAITS.Select(t => t.id).ToHashSet();
                ModAssets.badTraits = DUPLICANTSTATS.BADTRAITS.Select(t => t.id).ToHashSet();
                ModAssets.needTraits = DUPLICANTSTATS.NEEDTRAITS.Select(t => t.id).ToHashSet();
                ModAssets.vacillatorTraits = DUPLICANTSTATS.GENESHUFFLERTRAITS.Select(t => t.id).ToHashSet();

                Integration.TwitchIntegration.DbInit.OnDbInit();

                ModAssets.LateLoadAssets();
            }
        }
    }
}
