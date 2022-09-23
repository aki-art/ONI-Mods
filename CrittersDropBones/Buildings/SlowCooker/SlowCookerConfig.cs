using FUtility;
using System.Collections.Generic;
using System.Linq;
using TUNING;
using UnityEngine;
using static ComplexRecipe;

namespace CrittersDropBones.Buildings.SlowCooker
{
    public class SlowCookerConfig : IBuildingConfig
    {
        public const string ID = "CDB_Cooker";

        public override BuildingDef CreateBuildingDef()
        {
            var def = BuildingTemplates.CreateBuildingDef(
               ID,
               2,
               3,
               "cdb_cooker_kanim",
               BUILDINGS.HITPOINTS.TIER2,
               BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER4,
               BUILDINGS.CONSTRUCTION_MASS_KG.TIER3,
               MATERIALS.ALL_METALS,
               BUILDINGS.MELTING_POINT_KELVIN.TIER2,
               BuildLocationRule.OnFloor,
               DECOR.PENALTY.TIER0,
               NOISE_POLLUTION.NONE
           );

            def.Floodable = true;
            def.Overheatable = false;

            def.AudioCategory = Consts.AUDIO_CATEGORY.HOLLOWMETAL;
            def.ViewMode = OverlayModes.Power.ID;

            def.RequiresPowerInput = true;
            def.EnergyConsumptionWhenActive = .40f;
            def.SelfHeatKilowattsWhenActive = 1f;

            def.DefaultAnimState = "off";

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<CopyBuildingSettings>().copyGroupTag = ID;

            go.AddOrGet<DropAllWorkable>();
            //go.AddOrGet<BuildingComplete>().isManuallyOperated = false;

            go.AddOrGet<Stirrable>();

            var stirrableWorkable = go.AddOrGet<StirrableWorkable>();
            stirrableWorkable.workTime = 20f;
            stirrableWorkable.AnimOffset = new Vector3(-1f, 0f, 0f);

            /*
            var cookerWorkable = go.AddOrGet<CookerWorkable>();
            cookerWorkable.workTime = 20f;
            cookerWorkable.overrideAnims = new KAnimFile[]
            {
                Assets.GetAnim("cooker_interact_anim_kanim")
            };
            */

            var cooker = go.AddOrGet<ComplexFabricator>();
            cooker.heatedTemperature = 353.15f;
            cooker.duplicantOperated = false;
            cooker.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;

            go.AddOrGet<FabricatorIngredientStatusManager>();

            BuildingTemplates.CreateComplexFabricatorStorage(go, cooker);

            ConfigureRecipes();

            // emit Steam
            var emitter = go.AddComponent<BuildingElementEmitter>();
            emitter.emitRate = 0.01f;
            emitter.temperature = 378.15f;
            emitter.element = SimHashes.Steam;
            emitter.modifierOffset = new Vector2(1f, 3f);

            Prioritizable.AddRef(go);
        }

        // These are read in from a .json
        private void ConfigureRecipes()
        {
            // TODO: sanity checks
            foreach (var recipe in Mod.Recipes.Recipes)
            {
                var inputs = recipe.Inputs.Select(i => new RecipeElement(i.ID, i.Amount)).ToArray();
                var outputs = recipe.Outputs.Select(o => new RecipeElement(o.ID, o.Amount)).ToArray();

                CreateRecipe(ID, inputs, outputs, recipe.Description);
            }
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<EnergyConsumer>();
            go.AddOrGet<LoopingSounds>();

            //go.AddOrGetDef<PoweredController.Def>();
        }

        public ComplexRecipe CreateRecipe(string fabricatorID, RecipeElement[] input, RecipeElement[] output, string description, float time = 40f)
        {
            var recipeID = ComplexRecipeManager.MakeRecipeID(fabricatorID, input, output);

            var desc = description;

            if(Strings.TryGet(description, out var str))
            {
                desc = str;
            }

            var recipe = new ComplexRecipe(recipeID, input, output)
            {
                time = time,
                description = desc,
                nameDisplay = RecipeNameDisplay.Result,
                fabricators = new List<Tag> { TagManager.Create(fabricatorID) }
            };

            return recipe;
        }
    }
}
