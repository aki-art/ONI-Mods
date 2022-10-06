using TUNING;
using UnityEngine;
using static Workable;

namespace Slag.Content.Buildings.Spinner
{
    public class Spinner : ComplexFabricator
    {
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            choreType = Db.Get().ChoreTypes.Compound;
            fetchChoreTypeIdHash = Db.Get().ChoreTypes.DoctorFetch.IdHash;
            sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
            duplicantOperated = true;
            heatedTemperature = 400f;

        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            workable.workTime = 10.200001f;
            workable.trackUses = true;
            workable.workLayer = Grid.SceneLayer.BuildingUse;
            workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Processing;
            workable.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
            workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
            workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
            workable.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
            workable.overrideAnims = new KAnimFile[]
            {
                Assets.GetAnim("anim_interacts_ore_scrubber_kanim")
            };

            workable.OnWorkableEventCB += OnWorkableEvent;

            workable.AnimOffset = new Vector3(-1f, 0f, 0f);
        }

        private void OnWorkableEvent(Workable workable, WorkableEvent evt)
        {
            if (workable.worker == null)
            {
                return;
            }

            if (evt == WorkableEvent.WorkStarted)
            {
                workable.worker.GetComponent<KBatchedAnimController>().Offset += new Vector3(0.33f, 0);
            }
            else
            {
                workable.worker.GetComponent<KBatchedAnimController>().Offset -= new Vector3(0.33f, 0);
            }
        }
    }
}
