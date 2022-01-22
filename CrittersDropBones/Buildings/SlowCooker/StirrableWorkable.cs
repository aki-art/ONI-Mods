using FUtility;
using System;
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
			worker.FindOrAdd<StirInteractAnim>().enabled = true;

			Log.Debuglog("anims");
			for (int i = 0; i < kbac.AnimFiles.Length; i++)
            {
                KAnimFile anims = kbac.AnimFiles[i];
                Log.Debuglog(i, anims.name, anims.batchTag);
            }
		}

		protected override void OnStopWork(Worker worker)
		{
			if(worker.gameObject.TryGetComponent(out StirInteractAnim overrideAnim))
            {
				overrideAnim.enabled = false;
            }
		}
	}
}
