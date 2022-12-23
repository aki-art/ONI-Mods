using FUtility;
using FUtility.Components;
using HarmonyLib;
using KSerialization;
using System;
using UnityEngine;

namespace DecorPackB.Content.Scripts
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

        [Serialize]
        public float aptitude;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            Mod.restorers.Add(this);
            // Subscribe((int)GameHashes.SaveGameReady, OnLoadGame);
        }

        private void OnLoadGame(object obj)
        {
            if (restoreSkill)
            {
                resume.MasteryBySkillID.Add(skillId, true);
            }
        }

        public void BeforeMinionResumeSpawn()
        {
            if (restoreSkill)
            {
                Log.Debuglog("RESTOE SKILL");
                resume.MasteryBySkillID.Add(skillId, true);
            }
        }

        public override void OnSaveGame()
        {
            restoreSkill = resume.HasMasteredSkill(skillId);

            foreach (var apt in resume.AptitudeBySkillGroup)
            {
                var key = Db.Get().Skills.resources.FindIndex(r => r.Id == apt.Key);
                if (key > -1)
                {
                    Log.Debuglog(apt.Key, apt.Value);
                }
                else
                {
                    Log.Debuglog("Not a skill id");
                }
            }

            if (restoreSkill)
            {
                // Add directly to the dictionary, instead of calling UnmasterSkill; side effects of actually unmastering are not wanted
                resume.MasteryBySkillID.Remove(skillId);
                if (resume.AptitudeBySkillGroup.ContainsKey(skillId))
                {

                    Log.Debuglog("HAD APTITUDE" + resume.AptitudeBySkillGroup[skillId]);
                    aptitude = resume.AptitudeBySkillGroup[skillId];
                    resume.AptitudeBySkillGroup.Remove(skillId);
                }
                // resume.UnmasterSkill(skillId);
            }
        }

        public override void OnRestore()
        {
            Log.Debuglog($"ON RESTORE restoreSkill: {restoreSkill} hasSkill: {hasSkill}");
            if (restoreSkill)
            {
                resume.MasteryBySkillID.Add(skillId, true);

                if (aptitude > 0f)
                {
                    resume.AptitudeBySkillGroup[skillId] = aptitude;
                }
                //resume.MasterSkill(skillId);
                // ManagementMenu.Instance.GetIn

                //         this.skillWidgets[skill.Id].GetComponent<SkillWidget>().Refresh(skill.Id);
            }
        }
    }
}
