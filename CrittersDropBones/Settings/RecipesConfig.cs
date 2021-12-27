using CrittersDropBones.Items;
using FUtility.SaveData;
using System.Collections.Generic;

namespace CrittersDropBones.Settings
{
    public class RecipesConfig : IUserSetting
    {

        public class FRecipe
        {
            public string Description { get; set; }

            public float Time { get; set; } = 40f;

            public FRecipeElement[] Inputs { get; set; }

            public FRecipeElement[] Outputs { get; set; }
        }


        public class FRecipeElement
        {
            public FRecipeElement(Tag iD, float amount)
            {
                ID = iD.ToString();
                Amount = amount;
            }

            public string ID { get; set; }
            public float Amount { get; set; }
        }

        public List<FRecipe> Recipes { get; set; } = new List<FRecipe>()
        {
            // Soup Stock
            new FRecipe()
            {
                Description = STRINGS.ITEMS.FOOD.CDB_SOUPSTOCK.DESC,
                Time = 60f,
                Inputs = new FRecipeElement[] {
                    new FRecipeElement(GameTags.Water, 11),
                    new FRecipeElement(MediumBoneConfig.ID, 5)
                },
                Outputs = new FRecipeElement[] {
                    new FRecipeElement(SoupStockConfig.ID, 10)
                }
            },

            // Super Hot Soup
            new FRecipe()
            {
                Description = STRINGS.ITEMS.FOOD.CDB_SOUPSTOCK.DESC,
                Time = 1000f,
                Inputs = new FRecipeElement[] {
                    new FRecipeElement(GameTags.Water, 1),
                    new FRecipeElement("Magma", 1),
                    new FRecipeElement(SpiceNutConfig.ID, 10),
                },
                Outputs = new FRecipeElement[] {
                    new FRecipeElement(SuperHotSoupConfig.ID, 10)
                }
            },

            // Sludge
            new FRecipe()
            {
                Description = STRINGS.ITEMS.FOOD.CDB_SLUDGE.DESC,
                Time = 1000f,
                Inputs = new FRecipeElement[] {
                    new FRecipeElement(GameTags.Water, 1),
                    new FRecipeElement(GameTags.SlimeMold, 10)
                },
                Outputs = new FRecipeElement[] {
                    new FRecipeElement(SludgeConfig.ID, 10)
                }
            },

            // Grub Grub
            new FRecipe()
            {
                Description = STRINGS.ITEMS.FOOD.CDB_GRUBGRUB.DESC,
                Time = 1000f,
                Inputs = new FRecipeElement[] {
                    new FRecipeElement(GameTags.Water, 1),
                    new FRecipeElement(WormBasicFruitConfig.ID, 10),
                    new FRecipeElement(LettuceConfig.ID, 10),
                },
                Outputs = new FRecipeElement[] {
                    new FRecipeElement(GrubGrubConfig.ID, 10)
                }
            },

            // Vegetable Soup
            new FRecipe()
            {
                Description = STRINGS.ITEMS.FOOD.CDB_VEGETABLESOUP.DESC,
                Time = 1000f,
                Inputs = new FRecipeElement[] {
                    new FRecipeElement(GameTags.Water, 10000),
                    new FRecipeElement(SeaLettuceConfig.ID, 10),
                    new FRecipeElement(BasicSingleHarvestPlantConfig.ID, 10),
                    new FRecipeElement(MushroomConfig.ID, 10),
                },
                Outputs = new FRecipeElement[] {
                    new FRecipeElement(VegetableSoupConfig.ID, 10)
                }
            },

            // Egg Drop Soup
            new FRecipe()
            {
                Description = STRINGS.ITEMS.FOOD.CDB_EGGDROPSOUP.DESC,
                Time = 1000f,
                Inputs = new FRecipeElement[] {
                    new FRecipeElement(GameTags.Water, 10000),
                    new FRecipeElement(RawEggConfig.ID, 7)
                },
                Outputs = new FRecipeElement[] {
                    new FRecipeElement(EggDropSoupConfig.ID, 10)
                }
            },
        };
    }
}
