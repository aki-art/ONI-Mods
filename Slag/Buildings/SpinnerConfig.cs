using System.Collections.Generic;
using TUNING;
using UnityEngine;
using FUtility;
using static ComplexRecipe;

namespace Slag.Buildings
{
	//[StringsPath(typeof(SlagStrings.SLAGSTRINGS.BUILDINGS.PREFABS.SPINNER))]
	//[BuildMenu("Base")]
	class SpinnerConfig : IBuildingConfig, IModdedBuilding
	{

		public const string ID = "Spinner";
		public static LocString NAME = "Spinner";
		public static LocString DESC = "Spins stuff.";
		public static LocString EFFECT = "Spinner desc";

		public MBInfo Info => new MBInfo(ID, "Base");

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef def = BuildingTemplates.CreateBuildingDef(
				id: ID,
				width: 4,
				height: 4,
				anim: "rockrefinery_kanim",
				hitpoints: 30,
				construction_time: 60f,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER5,
				construction_materials: MATERIALS.ALL_METALS,
				melting_point: 2400f,
				build_location_rule: BuildLocationRule.OnFloor,
				decor: TUNING.BUILDINGS.DECOR.PENALTY.TIER2,
				noise: NOISE_POLLUTION.NOISY.TIER6,
				temperature_modification_mass_scale: 0.2f);

			def.RequiresPowerInput = true;
			def.EnergyConsumptionWhenActive = 240f;
			def.SelfHeatKilowattsWhenActive = 16f;
			def.ViewMode = OverlayModes.Power.ID;
			def.AudioCategory = "HollowMetal";
			def.AudioSize = "large";
			return def;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddOrGet<DropAllWorkable>();
			go.AddOrGet<BuildingComplete>().isManuallyOperated = true;

			ComplexFabricator refinery = go.AddOrGet<ComplexFabricator>();
			refinery.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
			refinery.duplicantOperated = true;

			go.AddOrGet<FabricatorIngredientStatusManager>();
			go.AddOrGet<CopyBuildingSettings>();

			ComplexFabricatorWorkable workable = go.AddOrGet<ComplexFabricatorWorkable>();
			BuildingTemplates.CreateComplexFabricatorStorage(go, refinery);
			workable.overrideAnims = new KAnimFile[] { Assets.GetAnim("anim_interacts_rockrefinery_kanim") };
			workable.workingPstComplete = new HashedString[] { "working_pst_complete" };

			AddRecipe(new RecipeElement(ElementLoader.FindElementByName("Slag").tag, 5f), new RecipeElement(SlagWoolConfig.ID, 5f), "Desc", 1);
			AddRecipe(new RecipeElement(PrickleFruitConfig.ID, 1f), new RecipeElement(Food.CottonCandyConfig.ID, 1f), "Desc", 2);
			AddRecipe(new RecipeElement(ForestForagePlantConfig.ID, 1f), new RecipeElement(Food.CottonCandyConfig.ID, 1f), "Desc", 3);
			AddRecipe(new RecipeElement(ColdWheatConfig.ID, 1f), new RecipeElement(Food.NoodlesConfig.ID, 1f), "Desc", 4);

		}

		public override void ConfigurePost(BuildingDef def)
		{
			base.ConfigurePost(def);
		}

		public static void AddRecipe(RecipeElement input, RecipeElement output, string desc, int sortOrder = 0, float time = 40f )
		{
			var i = new RecipeElement[] { input };
			var o = new RecipeElement[] { output };

			string recipeID = ComplexRecipeManager.MakeRecipeID(ID, i, o);

			var recipe = new ComplexRecipe(recipeID, i, o)
			{
				time = time,
				description = desc,
				nameDisplay = RecipeNameDisplay.IngredientToResult,
				fabricators = new List<Tag> { TagManager.Create(ID) }
			};

			//recipe.sortOrder = sortOrder;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			SymbolOverrideControllerUtil.AddToPrefab(go);

			go.GetComponent<KPrefabID>().prefabSpawnFn += delegate (GameObject game_object)
			{
				ComplexFabricatorWorkable workable = game_object.GetComponent<ComplexFabricatorWorkable>();
				workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Processing;
				workable.AttributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
				workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
				workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
				workable.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
			};
			Log.Info("Trying to add recipes");
			foreach (GameObject cloth in Assets.GetPrefabsWithTag(GameTags.Clothes))
			{
				Log.Info(cloth.PrefabID());
				Log.Info(cloth.name);
				AddRecipe(new RecipeElement(cloth.PrefabID(), 1f), new RecipeElement(BasicFabricConfig.ID, 1f), "Desc");
			}
			Log.Info(Assets.Prefabs.Count);
		}
	}
}
