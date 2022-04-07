using FUtility;
using TUNING;

namespace CrittersDropBones.Buildings.SlowCooker
{
    public class StirrableWorkable : Workable
    {
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            attributeConverter = Db.Get().AttributeConverters.CookingSpeed;
            attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
            skillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
            skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
        }

        protected override void OnStartWork(Worker worker)
        {
            var kbac = worker.GetComponent<KBatchedAnimController>();

            Log.Debuglog("anims");
            for (var i = 0; i < kbac.AnimFiles.Length; i++)
            {
                var anims = kbac.AnimFiles[i];
                Log.Debuglog(i, anims.name, anims.batchTag);
            }
        }

    }
}
