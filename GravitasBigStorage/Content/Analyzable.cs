using FUtility;
using KSerialization;
using System;
using TUNING;
using static STRINGS.UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN;

namespace GravitasBigStorage.Content
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class Analyzable : Workable, ISidescreenButtonControl
    {
        [Serialize] private bool studied;
        [Serialize]  private bool markedForStudy;

        private Guid statusItemGuid;
        private Chore chore;

        public string SidescreenButtonText => studied
                    ? (string)STUDIED_BUTTON
                    : markedForStudy ? (string)PENDING_BUTTON : (string)SEND_BUTTON;

        public string SidescreenButtonTooltip => studied
                    ? (string)STUDIED_STATUS
                    : markedForStudy ? (string)PENDING_STATUS : (string)SEND_STATUS;

#if DEBUG
        private const float WORK_TIME = 300f;
#else
        private const float WORK_TIME = 1800f;
#endif

        protected override void OnPrefabInit()
        {
            overrideAnims = new []
            {
                Assets.GetAnim( "anim_use_machine_kanim")
            };

            faceTargetWhenWorking = true;
            synchronizeAnims = false;
            workerStatusItem = Db.Get().DuplicantStatusItems.Studying;
            resetProgressOnStop = false;
            requiredSkillPerk = Db.Get().SkillPerks.CanStudyWorldObjects.Id;
            attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
            attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
            skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
            skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;

            SetWorkTime(WORK_TIME);
        }

        protected override void OnSpawn()
        {
            Refresh();
        }

        private void ToggleStudyChore()
        {
            markedForStudy = !markedForStudy;
            Refresh();
        }

        public void Refresh()
        {
            if (isLoadingScene)
            {
                return;
            }

            TryGetComponent(out KSelectable kSelectable);

            if (studied)
            {
                statusItemGuid = kSelectable.ReplaceStatusItem(statusItemGuid, Db.Get().MiscStatusItems.Studied);
                requiredSkillPerk = null;
                //UpdateStatusItem();
            }
            else
            {
                if (markedForStudy)
                {
                    chore ??= new WorkChore<Analyzable>(Db.Get().ChoreTypes.Research, this, only_when_operational: false);
                    statusItemGuid = kSelectable.ReplaceStatusItem(statusItemGuid, Db.Get().MiscStatusItems.AwaitingStudy);
                }
                else
                {
                    CancelChore();
                    statusItemGuid = kSelectable.RemoveStatusItem(statusItemGuid);
                }
            }
        }

        public void CancelChore()
        {
            if (chore == null)
            {
                return;
            }

            chore.Cancel("Analyzable.CancelChore");
            chore = null;
        }

        protected override void OnCompleteWork(Worker worker)
        {
            base.OnCompleteWork(worker);
            studied = true;
            chore = null;
            Log.Debuglog("Work complete");
            Refresh();
            UnlockStorage();
        }

        private void UnlockStorage()
        {
            throw new NotImplementedException();
        }

        public int ButtonSideScreenSortOrder() => 20;

        public void OnSidescreenButtonPressed() => ToggleStudyChore();

        public void SetButtonTextOverride(ButtonMenuTextOverride textOverride) {  }

        public bool SidescreenButtonInteractable() => true;

        public bool SidescreenEnabled() => !studied;

        public string SidescreenTitleKey => "STRINGS.UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.TITLE";
    }
}
