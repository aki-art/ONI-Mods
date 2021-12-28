using FUtility;
using KSerialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DecorPackB.STRINGS.BUILDINGS.PREFABS.DECORPACKB_FOSSILDISPLAY;

namespace DecorPackB.Buildings.FossilDisplay
{
    public class Assemblable : Artable
    {
        [Serialize]
        private string currentStage;

        private WorkChore<Assemblable> chore;
        private static KAnimFile[] sculptureOverrides;

        private const string DEFAULT_STAGE_ID = "Default";
        //[MyCmpReq] private Buildings.FossilStand fossilStand;

        private static readonly Dictionary<string, string> descriptions = new Dictionary<string, string>()
        {
            { "Default", "Default" },
            { "Parasaur", VARIANT.PARASAUROLOPHUS.DESC },
            { "Pacu", VARIANT.PACU.DESC },
            { "Human", VARIANT.HUMAN.DESC },
            { "Trilobite", VARIANT.TRILOBITE.DESC },
            { "Spider", VARIANT.SPIDER.DESC },
            { "Volgus", VARIANT.VOLGUS.DESC },
            { "Beefalo", VARIANT.BEEFALO.DESC },
            { "HellHound", VARIANT.HELLHOUND.DESC }
        };

        protected Assemblable()
        {
            faceTargetWhenWorking = true;
        }

        public string GetDescription()
        {
            Log.Debuglog("Current stage is", currentStage);
            return descriptions.TryGetValue(currentStage, out string desc) ? desc : "";
        }

        protected override void OnPrefabInit()
        {
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

            base.OnSpawn();
        }

        private void CreateChore()
        {
            chore = new WorkChore<Assemblable>(Db.Get().ChoreTypes.Research, this);
            chore.AddPrecondition(ChorePreconditions.instance.ConsumerHasTrait, requiredSkillPerk);
        }

        protected override void OnCompleteWork(Worker worker)
        {
            Stage stage = stages.Where(s => IsValidStageForWorker(worker, s)).ToList().GetRandom();

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
                new EmoteChore(worker.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, "anim_cheer_kanim", new HashedString[3]
                {
                    "cheer_pre",
                    "cheer_loop",
                    "cheer_pst"
                }, null);
            }
            else
            {
                new EmoteChore(worker.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, "anim_disappointed_kanim", new HashedString[3]
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

        public override void SetStage(string stage_id, bool skip_effect)
        {
            base.SetStage(stage_id, skip_effect);

            currentStage = stage_id;
            Stage stage = stages[0];
            for (int i = 0; i < stages.Count; i++)
            {
                if (stages[i].id == stage_id)
                {
                    stage = stages[i];
                    break;
                }
            }

            if (skip_effect || !(CurrentStage != "Default"))
            {
                return;
            }

            KBatchedAnimController effect = FXHelpers.CreateEffect("sculpture_fx_kanim", transform.GetPosition(), transform);
            effect.destroyOnAnimComplete = true;
            effect.transform.SetLocalPosition(new Vector3(0.5f, -0.5f));
            effect.Play("poof", KAnim.PlayMode.Once, 1f, 0.0f);

            //Trigger((int)ModHashes.OnStageSet);
            //fossilStand.CreateInspiredReactable(stage.statusItem);
        }
    }
}