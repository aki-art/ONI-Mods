using TUNING;

namespace CrittersDropBones.Buildings.SlowCooker
{
    public class Cooker : ComplexFabricator, IGameObjectEffectDescriptor
    {
        private StatesInstance smi;
        public static readonly Operational.Flag operationalFlag = new Operational.Flag("cooking_pot", Operational.Flag.Type.Requirement);

        protected override void OnSpawn()
        {
            base.OnSpawn();

            workable.requiredSkillPerk = Db.Get().SkillPerks.CanElectricGrill.Id;
            workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Cooking;
            workable.overrideAnims = new []
            {
                Assets.GetAnim("cooker_interact_anim_kanim")
            };
            workable.AttributeConverter = Db.Get().AttributeConverters.CookingSpeed;
            workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
            workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
            workable.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;

            smi = new StatesInstance(this);
            smi.StartSM();
        }

        public class States : GameStateMachine<States, StatesInstance, Cooker>
        {
            public override void InitializeStates(out BaseState default_state)
            {
                default_state = root;
            }
        }

        public class StatesInstance : GameStateMachine<States, StatesInstance, Cooker, object>.GameInstance
        {
            public StatesInstance(Cooker master) : base(master) 
            {
            }
        }
    }
}
