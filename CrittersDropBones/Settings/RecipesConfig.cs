using CrittersDropBones.Integration.SpookyPumpkin;
using CrittersDropBones.Items;
using FUtility.SaveData;
using System.Collections.Generic;
using static CrittersDropBones.RecipeUtil;

namespace CrittersDropBones.Settings
{
    public class RecipesConfig : IUserSetting
    {
        public List<FRecipe> Recipes { get; set; } = new List<FRecipe>()
        {
            // Soup Stock
            new FRecipe()
            {
                Description = "STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_SOUPSTOCK.DESC",
                SortOrder = 0,
                Inputs = new [] 
                {
                    new FRecipeElement(GameTags.Water, 11),
                    new FRecipeElement(BoneConfig.ID, 5)
                },
                Outputs = new [] 
                {
                    new FRecipeElement(SoupStockConfig.ID, 10)
                }
            },

            // Super Hot Soup
            new FRecipe()
            {
                Description = "STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_SUPERHOTSOUP.DESC",
                Inputs = new [] 
                {
                    new FRecipeElement(GameTags.Water, 1),
                    new FRecipeElement(SpiceNutConfig.ID, 10),
                },
                Outputs = new [] 
                {
                    new FRecipeElement(SuperHotSoupConfig.ID, 10)
                }
            },

            // Fish Soup
            new FRecipe()
            {
                Description = "STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_FISHSOUP.DESC",
                Inputs = new []
                {
                    new FRecipeElement(SoupStockConfig.ID, 1),
                    new FRecipeElement(FishMeatConfig.ID, 1),
                    new FRecipeElement(FishBoneConfig.ID, 1),
                },
                Outputs = new []
                {
                    new FRecipeElement(FishSoupConfig.ID, 10)
                }
            },

            // Sludge
            new FRecipe()
            {
                Description = "STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_SLUDGE.DESC",
                Inputs = new [] 
                {
                    new FRecipeElement(GameTags.Water, 1),
                    new FRecipeElement(GameTags.SlimeMold, 10)
                },
                Outputs = new [] 
                {
                    new FRecipeElement(SludgeConfig.ID, 10)
                }
            },

            // Grub Grub
            new FRecipe()
            {
                Description = "STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_GRUBGRUB.DESC",
                Inputs = new [] 
                {
                    new FRecipeElement(GameTags.Water, 1),
                    new FRecipeElement(WormBasicFruitConfig.ID, 10),
                    new FRecipeElement(LettuceConfig.ID, 10),
                },
                Outputs = new [] 
                {
                    new FRecipeElement(GrubGrubConfig.ID, 10)
                }
            },

            // Vegetable Soup
            new FRecipe()
            {
                Description = "STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_VEGETABLESOUP.DESC",
                Inputs = new [] 
                {
                    new FRecipeElement(GameTags.Water, 10000),
                    new FRecipeElement(SeaLettuceConfig.ID, 10),
                    new FRecipeElement(BasicSingleHarvestPlantConfig.ID, 10),
                    new FRecipeElement(MushroomConfig.ID, 10),
                },
                Outputs = new [] 
                {
                    new FRecipeElement(VegetableSoupConfig.ID, 10)
                }
            },
            
            // Fruit Soup Vanilla - Berry
            new FRecipe()
            {
                Description = "STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_FRUITSOUP.DESC",
                Inputs = new []
                {
                    new FRecipeElement(GameTags.Water, 1000),
                    new FRecipeElement(PrickleFruitConfig.ID, 4)
                },
                Outputs = new []
                {
                    new FRecipeElement(FruitSoupConfig.ID, 1)
                },
                Dlc = DlcManager.AVAILABLE_VANILLA_ONLY
            },
            
            // Fruit Soup Vanilla - Hexalent
            new FRecipe()
            {
                Description = "STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_FRUITSOUP.DESC",
                Inputs = new []
                {
                    new FRecipeElement(GameTags.Water, 1000),
                    new FRecipeElement(ForestForagePlantConfig.ID, 4)
                },
                Outputs = new []
                {
                    new FRecipeElement(FruitSoupConfig.ID, 1)
                },
                Dlc = DlcManager.AVAILABLE_VANILLA_ONLY
            },
            
            // Fruit Soup Vanilla - Palmera
            new FRecipe()
            {
                Description = "STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_FRUITSOUP.DESC",
                Inputs = new []
                {
                    new FRecipeElement(GameTags.Water, 1000),
                    new FRecipeElement("PalmeraBerry", 4)
                },
                Outputs = new []
                {
                    new FRecipeElement(FruitSoupConfig.ID, 1)
                },
                Dlc = DlcManager.AVAILABLE_VANILLA_ONLY
            },
            
            // Fruit Soup DLC - Berry
            new FRecipe()
            {
                Description = "STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_FRUITSOUP.DESC",
                Inputs = new []
                {
                    new FRecipeElement(GameTags.Water, 1000),
                    new FRecipeElement(PrickleFruitConfig.ID, 2),
                    new FRecipeElement(SimHashes.Sucrose.CreateTag(), 5),
                },
                Outputs = new []
                {
                    new FRecipeElement(FruitSoupConfig.ID, 1)
                },
                Dlc = DlcManager.AVAILABLE_EXPANSION1_ONLY
            },

            // Fruit Soup DLC - Hexalent
            new FRecipe()
            {
                Description = "STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_FRUITSOUP.DESC",
                Inputs = new []
                {
                    new FRecipeElement(GameTags.Water, 1000),
                    new FRecipeElement(ForestForagePlantConfig.ID, 2),
                    new FRecipeElement(SimHashes.Sucrose.CreateTag(), 5),
                },
                Outputs = new []
                {
                    new FRecipeElement(FruitSoupConfig.ID, 1)
                },
                Dlc = DlcManager.AVAILABLE_EXPANSION1_ONLY
            },

            // Fruit Soup DLC - Palmera
            new FRecipe()
            {
                Description = "STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_FRUITSOUP.DESC",
                Inputs = new []
                {
                    new FRecipeElement(GameTags.Water, 1000),
                    new FRecipeElement("PalmeraBerry", 2),
                    new FRecipeElement(SimHashes.Sucrose.CreateTag(), 5),
                },
                Outputs = new []
                {
                    new FRecipeElement(FruitSoupConfig.ID, 1)
                },
                Dlc = DlcManager.AVAILABLE_EXPANSION1_ONLY
            },

            new FRecipe()
            {
                Description = "STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_PUMPKINSOUP.DESC",
                SortOrder = 2,
                Inputs = new [] 
                {
                    new FRecipeElement(GameTags.Water, 10000),
                    new FRecipeElement("SP_Pumpkin", 2),
                    new FRecipeElement(BasicPlantFoodConfig.ID, 5),
                },
                Outputs = new [] 
                {
                    new FRecipeElement(PumpkinSoupConfig.ID, 10)
                }
            },
            /*
            // Egg Drop Soup
            new FRecipe()
            {
                Description = "STRINGS.ITEMS.FOOD.CRITTERSDROPBONES_EGGDROPSOUP.DESC",
                Time = 1000f,
                Inputs = new FRecipeElement[] {
                    new FRecipeElement(GameTags.Water, 10000),
                    new FRecipeElement(RawEggConfig.ID, 7)
                },
                Outputs = new FRecipeElement[] {
                    new FRecipeElement(EggDropSoupConfig.ID, 10)
                }
            },
            */
        };
    }
}
