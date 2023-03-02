using FUtility;
using KSerialization;
using System;
using TUNING;
using static STRINGS.UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN;

namespace GravitasBigStorage.Content
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class Analyzable : Workable//, ISidescreenButtonControl
    {
        private const float WORK_TIME = 1800f;

        [MyCmpReq] KSelectable kSelectable;

        [Serialize] public bool storyTraitUnlocked;
        [Serialize] public bool studied;
        [Serialize] public bool markedForStudy;
        [Serialize] public bool canBeResearched;

        private Guid statusItemGuid;
        private Chore chore;

        private BuildingDef def;

        public string SidescreenButtonText => studied
                    ? STUDIED_BUTTON
                    : markedForStudy ? PENDING_BUTTON : SEND_BUTTON;

        public string SidescreenButtonTooltip => studied
                    ? STUDIED_STATUS
                    : markedForStudy ? PENDING_STATUS : SEND_STATUS;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            overrideAnims = new[]
            {
                Assets.GetAnim( "anim_use_machine_kanim")
            };

            faceTargetWhenWorking = true;
            synchronizeAnims = false;
            workerStatusItem = GBSStatusItems.beingStudied;
            resetProgressOnStop = false;
            requiredSkillPerk = Db.Get().SkillPerks.CanStudyWorldObjects.Id;
            attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
            attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
            skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
            skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;

            SetWorkTime(WORK_TIME);
        }

        public void RefreshSideScreen()
        {
            if (kSelectable == null || !kSelectable.IsSelected)
            {
                return;
            }

            DetailsScreen.Instance.Refresh(gameObject);
        }

        public bool CanBeResearched()
        {
            Log.Debuglog("Check research");
            canBeResearched = !Db.Get().TechItems.IsTechItemComplete(this.PrefabID().ToString());
            return !Db.Get().TechItems.IsTechItemComplete(this.PrefabID().ToString());
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            def = GetComponent<Building>().Def;
            Refresh();

            var storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.LonelyMinion.HashId);
            if (storyInstance != null && storyInstance.CurrentState == StoryInstance.State.COMPLETE)
            {
                Log.Debuglog("already unlocked");
                storyTraitUnlocked = true;
                RootMenu.Instance.Refresh();
                RefreshSideScreen();
            }

            Game.Instance.Subscribe(1594320620, OnUnlock);
        }

        private void OnUnlock(object obj)
        {
            storyTraitUnlocked = true;
            RefreshSideScreen();
            RootMenu.Instance.Refresh();
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

            if (studied)
            {
                statusItemGuid = kSelectable.ReplaceStatusItem(statusItemGuid, Db.Get().MiscStatusItems.Studied);
                requiredSkillPerk = null;
                UpdateStatusItem();
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

            if (kSelectable.IsSelected)
            {
                DetailsScreen.Instance.Refresh(gameObject);
            }
        }

        private void UnlockStorage()
        {
            var techInstance = Research.Instance.GetTechInstance(GBSTechs.BIG_BOY_STORAGE);

            if (techInstance.IsComplete())
            {
                return;
            }

            techInstance.Purchased();

            Game.Instance.Trigger((int)GameHashes.ResearchComplete, techInstance.tech);
        }

        public void OnSidescreenButtonPressed() => ToggleStudyChore();

        public bool SidescreenButtonInteractable() => true;

        public bool SidescreenEnabled() => true; // storyTraitUnlocked && !studied;// && CanBeResearched();

        public string SidescreenTitleKey => "STRINGS.UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.TITLE";
    }
}
