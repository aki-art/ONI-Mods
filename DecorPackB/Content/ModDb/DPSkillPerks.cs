using Database;
using DecorPackB.Content.Scripts;

namespace DecorPackB.Content.ModDb
{
    public class DPSkillPerks
    {
        public static SkillPerk CanFindTreasures;
        public const string CANFINDTREASURES_ID = "DecorPackB_CanFindTreasures";

        public static void Register(SkillPerks skillPerks)
        {
            CanFindTreasures = skillPerks.Add(new SkillPerk(CANFINDTREASURES_ID, "Treasure Finder", OnAddTreasureFinder, OnRemoveTreasureFinder, null));
        }

        private static void OnAddTreasureFinder(MinionResume resume)
        {
            var archeologistRestorer = resume.GetComponent<ArcheologistRestorer>();
            archeologistRestorer.hasSkill = true;
        }

        private static void OnRemoveTreasureFinder(MinionResume resume)
        {
            resume.GetComponent<ArcheologistRestorer>().hasSkill = false;
        }
    }
}
