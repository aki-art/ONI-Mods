using Database;
using HarmonyLib;

namespace DecorPackB.Content.Buildings.FossilDisplays
{
    public class Exhibition : Artable
    {
        private static AccessTools.FieldRef<Artable, string> userChosenTargetStage;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            var f_userChosenTargetStage = AccessTools.Field(typeof(Artable), "userChosenTargetStage");
            userChosenTargetStage = AccessTools.FieldRefAccess<Artable, string>( f_userChosenTargetStage);
            
            workerStatusItem = Db.Get().DuplicantStatusItems.Arting;
            attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
            skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
            requiredSkillPerk = Db.Get().SkillPerks.IncreaseLearningSmall.Id;

            SetWorkTime(Mod.DebugMode ? 8f : 80f);

            overrideAnims = new[] 
            { 
                Assets.GetAnim("anim_interacts_sculpture_kanim") 
            };

            synchronizeAnims = false;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            shouldShowSkillPerkStatusItem = true;
        }

        private ArtableStatusItem GetScientistSkill(Worker worker)
        {
            if (worker.TryGetComponent(out MinionResume resume))
            {
                if (resume.HasPerk(Db.Get().SkillPerks.AllowNuclearResearch.Id))
                {
                    return Db.Get().ArtableStatuses.LookingGreat;
                }
                else if (resume.HasPerk(Db.Get().SkillPerks.CanStudyWorldObjects.Id))
                {
                    return Db.Get().ArtableStatuses.LookingOkay;
                }
            }

            return Db.Get().ArtableStatuses.LookingUgly;
        }

        protected override void OnCompleteWork(Worker worker)
        {
            if(userChosenTargetStage(this) == null || userChosenTargetStage(this).IsNullOrWhiteSpace())
            {
                SetRandomStage(worker);
            }
            else
            {
                SetUserChosenStage();
            }

            shouldShowSkillPerkStatusItem = false;
            UpdateStatusItem(null);
            Prioritizable.RemoveRef(gameObject);
        }

        private void SetUserChosenStage()
        {
            SetStage(userChosenTargetStage(this), false);
            userChosenTargetStage(this) = null;
        }

        private void SetRandomStage(Worker worker)
        {
            var scientistSkill = GetScientistSkill(worker);
            var potentialStages = Db.GetArtableStages().GetPrefabStages(this.PrefabID());

            potentialStages.RemoveAll(stage => stage.statusItem.StatusType != scientistSkill.StatusType);
            var selectedStage = potentialStages.GetRandom();

            SetStage(selectedStage.id, false);
            EmoteOnCompletion(worker, selectedStage);
        }

        public override void SetStage(string stage_id, bool skip_effect)
        {
            base.SetStage(stage_id, skip_effect);

            var stage = Db.GetArtableStages().Get(CurrentStage);
            if(stage != null)
            {
                Trigger((int)ModHashes.FossilStageSet, stage.statusItem.StatusType);
            }
        }

        private static void EmoteOnCompletion(Worker worker, ArtableStage stage)
        {
            if (stage.cheerOnComplete)
            {
                new EmoteChore(worker.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, "anim_cheer_kanim", new HashedString[]
                {
                    "cheer_pre",
                    "cheer_loop",
                    "cheer_pst"
                }, null);
            }
            else
            {
                new EmoteChore(worker.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, "anim_disappointed_kanim", new HashedString[]
                {
                    "disappointed_pre",
                    "disappointed_loop",
                    "disappointed_pst"
                }, null);
            }
        }
    }
}
