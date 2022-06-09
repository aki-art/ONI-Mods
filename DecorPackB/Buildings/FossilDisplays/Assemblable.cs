using FUtility;
using KSerialization;
using System.Linq;
using UnityEngine;

namespace DecorPackB.Buildings.FossilDisplays
{
    public class Assemblable : Artable
    {
        [Serialize]
        private string currentStage;

        [SerializeField]
        public bool giant = false;

        private WorkChore<Assemblable> chore;
        private static KAnimFile[] sculptureOverrides;

        public const string DEFAULT_STAGE_ID = "Default";

        [Serialize]
        public string assemblerName;

        protected Assemblable()
        {
            faceTargetWhenWorking = true;
        }

        protected override void OnPrefabInit()
        {
            assemblerName = STRINGS.BUILDINGS.PREFABS.DECORPACKB_FOSSILDISPLAY.NOONE;

            base.OnPrefabInit();

            workerStatusItem = Db.Get().DuplicantStatusItems.Researching;
            attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
            skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
            requiredSkillPerk = Db.Get().SkillPerks.IncreaseLearningSmall.Id;
            SetWorkTime(8f); // TODO: 80 not 8

            if (sculptureOverrides == null)
            {
                sculptureOverrides = new KAnimFile[1] { Assets.GetAnim("anim_interacts_sculpture_kanim") };
            }

            overrideAnims = sculptureOverrides;
            synchronizeAnims = false;
        }

        protected override void OnSpawn()
        {

            if (string.IsNullOrEmpty(currentStage))
            {
                currentStage = DEFAULT_STAGE_ID;
            }

            SetStage(currentStage, true);
            shouldShowSkillPerkStatusItem = false;

            if (currentStage == DEFAULT_STAGE_ID)
            {
                shouldShowSkillPerkStatusItem = true;
                Prioritizable.AddRef(gameObject);
                CreateChore();
            }
            else
            {
                CreateReactable();
            }

            base.OnSpawn();
        }

        private void CreateChore()
        {
            chore = new WorkChore<Assemblable>(Db.Get().ChoreTypes.Research, this);
            chore.AddPrecondition(ChorePreconditions.instance.ConsumerHasTrait, requiredSkillPerk);
        }

        protected override void OnCompleteWork(Worker worker)
        {
            assemblerName = worker.GetProperName();

            var stage = stages.Where(s => IsValidStageForWorker(worker, s)).ToList().GetRandom();

            if (stage is null)
            {
                Log.Warning("No valid stage for Assemblable.");
                return;
            }

            SetStage(stage.id, false);
            EmoteOnCompletion(worker, stage);
            shouldShowSkillPerkStatusItem = false;
            UpdateStatusItem();

            Prioritizable.RemoveRef(gameObject);
        }

        private static void EmoteOnCompletion(Worker worker, Stage stage)
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

        private bool IsValidStageForWorker(Worker worker, Stage stage)
        {
            return stage.id != DEFAULT_STAGE_ID && stage.statusItem >= GetScientistSkill(worker);
        }

        private Status GetScientistSkill(Worker worker)
        {
            if (worker.TryGetComponent(out MinionResume resume))
            {
                if (resume.HasPerk(Db.Get().SkillPerks.AllowInterstellarResearch.Id))
                {
                    return Status.Great;
                }
                else if (resume.HasPerk(Db.Get().SkillPerks.CanStudyWorldObjects.Id))
                {
                    return Status.Okay;
                }
            }

            return Status.Ugly;
        }

        private void CreateReactable()
        {
            if (TryGetComponent(out MediumFossilDisplay fossilDisplay))
            {
                /*
                var effect = CurrentStatus switch
                {
                    Status.Ugly => ModAssets.Effects.INSPIRED_LOW,
                    Status.Ready => throw new System.NotImplementedException(),
                    Status.Okay => throw new System.NotImplementedException(),
                    Status.Great => throw new System.NotImplementedException(),
                    _ => throw new System.NotImplementedException()
                };*/

                var effect = CurrentStatus == Status.Ugly
                    ? ModAssets.Effects.INSPIRED_LOW
                    : giant ? ModAssets.Effects.INSPIRED_GIANT : ModAssets.Effects.INSPIRED_GOOD;

                fossilDisplay.CreateInspiredReactable(effect);
            }
        }

        public override void SetStage(string stage_id, bool skip_effect)
        {
            base.SetStage(stage_id, skip_effect);
            currentStage = stage_id;

            if (skip_effect || !(CurrentStage != "Default"))
            {
                return;
            }

            CreateSmokeFX();
            CreateReactable();
        }

        private void CreateSmokeFX()
        {
            var fx = FXHelpers.CreateEffect("sculpture_fx_kanim", transform.GetPosition(), transform);
            fx.destroyOnAnimComplete = true;
            fx.transform.SetLocalPosition(new Vector3(0.5f, -0.5f));
            fx.Play("poof", KAnim.PlayMode.Once, 1f, 0.0f);
        }
    }
}