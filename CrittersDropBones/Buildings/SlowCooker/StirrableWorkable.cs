using TUNING;

namespace CrittersDropBones.Buildings.SlowCooker
{
    public class StirrableWorkable : Workable
    {
        [MyCmpReq]
        private Stirrable stirrable;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            attributeConverter = Db.Get().AttributeConverters.CookingSpeed;
            attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
            skillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
            skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;

            overrideAnims = new[]
            {
                Assets.GetAnim("cooker_interact_anim_kanim")
            };

            workAnims = null;
            synchronizeAnims = false;
            SetWorkTime(60f);

            /*
            workAnims = new HashedString[]
            {
                "working_pre",
                "working_loop",
                "working_pst"
                //Assets.GetAnim("cooker_interact_anim_kanim")
            };*/
        }

        protected override void OnCompleteWork(Worker worker)
        {
            base.OnCompleteWork(worker);
            stirrable.CompleteStir();
        }

        protected override void OnStartWork(Worker worker)
        {
            stirrable.SetWorker(worker);
        }
    }
}
