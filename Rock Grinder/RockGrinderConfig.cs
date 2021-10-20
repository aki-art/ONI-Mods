using FUtility;
using System.Collections.Generic;
using TUNING;
using UnityEngine;
using static STRINGS.BUILDINGS.PREFABS.ROCKCRUSHER;
using static ComplexRecipe;
using System.Linq;

namespace RockGrinder
{
    public class RockGrinderConfig : IBuildingConfig, IModdedBuilding
    {
        public const string ID = "RG_RockGrinder";

        public MBInfo Info => new MBInfo(ID, "Base", "GenericSensors");

        public override BuildingDef CreateBuildingDef()
        {
            BuildingDef def = BuildingTemplates.CreateBuildingDef(
                id: ID,
                width: 3,
                height: 2,
                anim: "rockgrinder_kanim",
                hitpoints: BUILDINGS.HITPOINTS.TIER1,
                construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
                construction_mass: new float[] { BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0], BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0] },
                construction_materials: new string[] { "Transparent", "Metal" },
                melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER3,
                build_location_rule: BuildLocationRule.Tile,
                decor: BUILDINGS.DECOR.PENALTY.TIER2,
                noise: NOISE_POLLUTION.NONE);

            def.RequiresPowerInput = true;
            def.EnergyConsumptionWhenActive = 240f;
            def.SelfHeatKilowattsWhenActive = 16f;
            def.ViewMode = OverlayModes.Power.ID;
            def.AudioCategory = "HollowMetal";
            def.AudioSize = "large";
            def.ReplacementTags = new List<Tag>() { ID };

            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
            go.AddOrGet<DropAllWorkable>();

            ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
            fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
            fabricator.duplicantOperated = false;
            fabricator.outputOffset = Vector3.right;

            BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);
            go.AddOrGet<FabricatorIngredientStatusManager>();

            SimpleRecipe recipes = AdditionalRecipes.Read();

            var grinder = go.AddComponent<RockGrinder>();
            grinder.grindDupes = recipes.HurtFallenDuplicants;
            grinder.grindCritters = recipes.AllowCreatureGrinding;

            ConfigureRecipes(recipes);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGetDef<PoweredActiveController.Def>();
        }

        private void ConfigureRecipes(SimpleRecipe recipes)
        {

            // sand
            foreach (Element element in ElementLoader.elements.FindAll(e => e.HasTag(GameTags.Crushable)))
            {
                var result = new RecipeElement[1]
                {
                    new RecipeElement(SimHashes.Sand.CreateTag(), 100f)
                };

                RecipeUtil.CreateRecipe(ID, element.tag, 100f * recipes.MineralToSandRatio, result, RECIPE_DESCRIPTION);
            }

            // metals
            foreach (Element element in ElementLoader.elements.FindAll(e => e.IsSolid && e.HasTag(GameTags.Metal)))
            {
                Element lowTempTransition = element.highTempTransition.lowTempTransition;
                if (lowTempTransition != element)
                {
                    var result = new RecipeElement[1]
                    {
                        new RecipeElement(lowTempTransition.tag, 100f * recipes.OreToMetalRatio)
                    };

                    RecipeUtil.CreateRecipe(ID, element.tag, 100f, result, METAL_RECIPE_DESCRIPTION);
                }
            }

            // additional
            if (recipes.AdditionalRecipes != null)
                foreach (var recipe in recipes.AdditionalRecipes)
                {
                    var outputs = recipe.Value.Out.Select(o => new RecipeElement(o.Key, o.Value)).ToArray();
                    RecipeUtil.CreateRecipe(ID, recipe.Key, recipe.Value.In, outputs.ToArray(), RECIPE_DESCRIPTION);
                }
        }
    }
}
