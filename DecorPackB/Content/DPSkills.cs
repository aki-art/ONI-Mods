using Database;
using System.Collections.Generic;

namespace DecorPackB.Content
{
    internal class DPSkills
    {
        public static Skill archeology;
        public const string ARCHEOLOGY_ID = "DecorPackB_Archeology";

        public static void Register(Skills skills)
        {
            archeology = skills.Add(new Skill(
                ARCHEOLOGY_ID,
                "Archeology",
                "...",
                "",
                2,
                "hat_role_dpb_archeology",
                "skillbadge_role_mining1",
                Db.Get().SkillGroups.Mining.Id,
                new List<SkillPerk>
                {
                    DPSkillPerks.CanFindTreasures
                }, new List<string>()
                {
                    skills.Mining2.Id,
                    skills.Researching2.Id
                }));
        }
    }
}
