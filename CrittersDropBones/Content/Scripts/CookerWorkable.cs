using TUNING;

namespace CrittersDropBones.Content.Scripts
{
    public class CookerWorkable : ComplexFabricatorWorkable
    {
        public override void OnPrefabInit()
        {
            base.OnPrefabInit();
            attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
            attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
            skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
            skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
        }

        public override void OnStartWork(Worker worker)
        {
        }

        public override void OnStopWork(Worker worker)
        {
        }
    }
}
