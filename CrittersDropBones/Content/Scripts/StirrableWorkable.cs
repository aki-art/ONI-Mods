using TUNING;

namespace CrittersDropBones.Content.Scripts
{
    public class StirrableWorkable : Workable
    {
        public override void OnPrefabInit()
        {
            base.OnPrefabInit();

            attributeConverter = Db.Get().AttributeConverters.CookingSpeed;
            attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
            skillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
            skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;

            overrideAnims = new[]
            {
                //Assets.GetAnim("cooker_interact_anim_kanim")
                Assets.GetAnim("anim_interacts_cookstation_gourtmet_kanim")
            };

            workAnims = null;
            synchronizeAnims = false;
            SetWorkTime(10f);

            /*
            workAnims = new HashedString[]
            {
                "working_pre",
                "working_loop",
                "working_pst"
                //Assets.GetAnim("cooker_interact_anim_kanim")
            };*/
        }
    }
}
