using FUtility;
using FUtility.Components;
using KSerialization;
using System.Runtime.Serialization;
using UnityEngine;

namespace DecorPackB.Cmps
{
    // Prevents a soft-lock after uninstalling the mod by removing my modded skill before the game saves,
    // and then reapplying it immediately when saving is done
    [SerializationConfig(MemberSerialization.OptIn)]
    public class ArcheologistRestorer : Restorer
    {
        [MyCmpReq]
        private MinionResume resume;

        [SerializeField]
        public string skillId;

        [SerializeField]
        [Serialize]
        public bool restoreSkill;

        [Serialize]
        public bool hasSkill;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            Mod.restorers.Add(this);
        }

        public void BeforeMinionResumeSpawn()
        {
            if (hasSkill)
            {
                resume.MasteryBySkillID.Add(skillId, true);
            }
        }

        public override void OnSaveGame()
        {
            restoreSkill = resume.HasMasteredSkill(skillId);

            if (restoreSkill)
            {
                // Add directly to the dictionary, instead of calling UnmasterSkill; side effects of actually unmastering are not wanted
                // resume.MasteryBySkillID.Remove(skillId);
                resume.UnmasterSkill(skillId);
            }
        }

        public override void OnRestore()
        {
            Log.Debuglog($"ON RESTORE restoreSkill: {restoreSkill} hasSkill: {hasSkill}");
            if (restoreSkill)
            {
                //resume.MasteryBySkillID.Add(skillId, true);
                resume.MasterSkill(skillId);
            }
        }
    }
}
