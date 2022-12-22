using System.Collections.Generic;
using System.Linq;
using TUNING;

namespace PrintingPodRecharge.Content
{
    public class ModDb
    {
        public static Personality Meep;
        public static List<string> dupeSkillIds;

        public static void OnDbInit(Db db)
        {
            var attributes = Db.Get().Attributes;
            dupeSkillIds = new List<string>()
            {
                attributes.Athletics.Id,
                attributes.Strength.Id,
                attributes.Digging.Id,
                attributes.Construction.Id,
                attributes.Art.Id,
                attributes.Caring.Id,
                attributes.Learning.Id,
                attributes.Machinery.Id,
                attributes.Cooking.Id,
                attributes.Botanist.Id,
                attributes.Ranching.Id
            };

            if(DlcManager.FeatureRadiationEnabled())
            {
                dupeSkillIds.Add(attributes.SpaceNavigation.Id);
            }

            if(Mod.otherMods.IsBeachedHere)
            {
                dupeSkillIds.Add("Beached_Mineralogy");
            }

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
