using FUtility;
using TUNING;
using Twitchery.Content.Defs.Debris;
using Twitchery.Content.Defs.Foods;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Defs.Buildings
{
    public class SalesStandConfig : IBuildingConfig
    {
        public const string ID = "AkisExtraTwitchEvents_SalesStand";

        public override BuildingDef CreateBuildingDef()
        {
            var prefab = BuildingTemplates.CreateBuildingDef(
                ID,
                2,
                2,
                "farmtile_kanim",
                BUILDINGS.HITPOINTS.TIER1,
                BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER1,
                BUILDINGS.CONSTRUCTION_MASS_KG.TIER1,
                MATERIALS.ANY_BUILDABLE,
                BUILDINGS.MELTING_POINT_KELVIN.TIER3,
                BuildLocationRule.OnFloor,
                DECOR.BONUS.TIER3,
                NOISE_POLLUTION.NONE);

            return prefab;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            var fabricator = go.AddOrGet<SalesStand>();

            go.AddOrGet<FabricatorIngredientStatusManager>();
            BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);

            var fabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
            fabricatorWorkable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;

        }

        public override void DoPostConfigureComplete(GameObject go)
		{
			AddItem(ColdWheatBreadConfig.ID, 1f, 5f, "desc");
            AddItem(BurgerConfig.ID, 1f, 5f, "desc");
            AddItem(MeatConfig.ID, 1f, 5f, "desc");
            AddItem(CookedMeatConfig.ID, 1f, 5f, "desc");
            AddItem(CookedFishConfig.ID, 1f, 5f, "desc");
            AddItem(PizzaConfig.ID, 1f, 5f, "desc");
            AddItem(GoopParfaitConfig.ID, 1f, 5f, "desc");
            AddItem(Elements.Jello.ToString(), 1f, 5f, "desc");
		}

        private static void AddItem(string prefabId, float amount, float cost, string description)
        {
			RecipeBuilder.Create(ID, description, 10f)
				.Input(SimHashes.Gold.CreateTag(), cost)
				.Output(prefabId, amount)
				.Build();
		}
    }
}
