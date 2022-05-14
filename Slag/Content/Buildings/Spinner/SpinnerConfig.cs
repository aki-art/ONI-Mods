using FUtility;
using System;
using TUNING;
using UnityEngine;

namespace Slag.Content.Buildings.Spinner
{
    public class SpinnerConfig : IBuildingConfig
    {
        public const string ID = "Slag_Spinner";

        public override BuildingDef CreateBuildingDef()
        {
            var def = BuildingTemplates.CreateBuildingDef(
                ID, 
                2, 
                4, 
                "spinner_kanim", 
                BUILDINGS.HITPOINTS.TIER2, 
                BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
                BUILDINGS.CONSTRUCTION_MASS_KG.TIER2,  
                MATERIALS.ALL_METALS, 
                BUILDINGS.MELTING_POINT_KELVIN.TIER1, 
                BuildLocationRule.OnFloor, 
                BUILDINGS.DECOR.NONE, 
                default);

            def.RequiresPowerInput = true;
            def.EnergyConsumptionWhenActive = 240f;
            def.SelfHeatKilowattsWhenActive = 16f;

            def.ViewMode = OverlayModes.Power.ID;

            def.AudioCategory = AUDIO.CATEGORY.HOLLOW_METAL;
            def.AudioSize = AUDIO.SIZE.LARGE;

            return def;
        }
        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<DropAllWorkable>();
            go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
            go.AddOrGet<FabricatorIngredientStatusManager>();
            go.AddOrGet<CopyBuildingSettings>();

            var complexFabricator = go.AddOrGet<ComplexFabricator>();
            complexFabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
            complexFabricator.duplicantOperated = true;

            var complexFabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();

            BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);

            complexFabricatorWorkable.overrideAnims = new KAnimFile[]
            {
                Assets.GetAnim("anim_interacts_rockrefinery_kanim")
            };

            complexFabricatorWorkable.workingPstComplete = new HashedString[]
            {
                "working_pst_complete"
            };

            ConfigureRecipes();
        }

        private void ConfigureRecipes()
        {
            var slag = "Slag".ToTag();

            Utils.AddRecipe(
                ID,
                new ComplexRecipe.RecipeElement(slag, 50f),
                new ComplexRecipe.RecipeElement(Items.SlagWoolConfig.ID, 1f), 
                "Slag wool recipe desc.");
        }

        private static void AddSimpleRecipe(Tag tag1, float amount1, Tag tag1, float amount2)
        {

        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            SymbolOverrideControllerUtil.AddToPrefab(go);
            go.GetComponent<KPrefabID>().prefabSpawnFn += game_object =>
            {
                ComplexFabricatorWorkable component = game_object.GetComponent<ComplexFabricatorWorkable>();
                component.WorkerStatusItem = Db.Get().DuplicantStatusItems.Processing;
                component.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
                component.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
                component.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
                component.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
            };
        }
    }
}
