using Database;
using System.Collections.Generic;

namespace DecorPackB.Content.ModDb
{
    internal class DPSkills
    {
        public static Skill archeology;
        public const string ARCHEOLOGY_ID = "DecorPackB_Archeology";

        public static void Register(Skills skills)
        {
/*            if(Mod.isBeachedHere)
            {
                return;
            }*/
            archeology = skills.Add(new Skill(
                ARCHEOLOGY_ID,
                "Archeology",
                "...",
                "",
                2,
                "hat_role_dpb_archeology",
                "skillbadge_role_mining1",
                Db.Get().SkillGroups.Mining.Id,
                new ()
                {
                    DPSkillPerks.CanFindTreasures
                }, new ()
                {
                    skills.Mining2.Id,
                    skills.Researching2.Id
                }));
        }
    }
}
