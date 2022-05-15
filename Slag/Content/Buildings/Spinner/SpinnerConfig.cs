using FUtility;
using Slag.Content.Items;
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

            def.ForegroundLayer = Grid.SceneLayer.BuildingFront;

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<DropAllWorkable>();
            go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
            go.AddOrGet<FabricatorIngredientStatusManager>();
            go.AddOrGet<CopyBuildingSettings>();

            var complexFabricator = go.AddOrGet<Spinner>();

            BuildingTemplates.CreateComplexFabricatorStorage(go, complexFabricator);

            go.AddOrGet<ComplexFabricatorWorkable>();
            go.AddOrGet<FabricatorIngredientStatusManager>();

            ConfigureRecipes();
        }

        private void ConfigureRecipes()
        {
            var slag = "Slag".ToTag();

            RecipeBuilder.Create(ID, "...", 40f)
                .Input(slag, 50f)
                .Output(SlagWoolConfig.ID, 1f, ComplexRecipe.RecipeElement.TemperatureOperation.Heated)
                .Build();

            var cottonCandy = RecipeBuilder.Create(ID, "...", 10f)
                .Output(CottonCandyConfig.ID, 1f);

            if (DlcManager.IsExpansion1Active())
            {
                cottonCandy.Input(SimHashes.Sucrose.CreateTag(), 30f);
            }
            else
            {
                cottonCandy.Input(PrickleFruitConfig.ID, 3f);
            }

            cottonCandy.Build();

            RecipeBuilder.Create(ID, "...", 10f)
                .Input(SimHashes.Ice.CreateTag(), 10f)
                .Input(BasicPlantFoodConfig.ID, 2f)
                .Input(PrickleFruitConfig.ID, 2f)
                .Output(LiceCreamConfig.ID, 1f)
                .Build();
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            SymbolOverrideControllerUtil.AddToPrefab(go);
        }
    }
}
