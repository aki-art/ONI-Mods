using FUtility.SaveData;
using PrintingPodRecharge.Content.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrintingPodRecharge.Settings
{
    public class Recipes : IUserSetting
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
            public string ID { get; set; }

            public float Amount { get; set; }

            public FRecipeElement(Tag iD, float amount)
            {
                ID = iD.ToString();
                Amount = amount;
            }
        }

        public List<FRecipe> BioInks { get; set; } = new List<FRecipe>()
        {
            new FRecipe()
            {
                Description = "PrintingPodRecharge.STRINGS.ITEMS.GERMINATED_BIO_INK.DESC",
                Time = 40f,
                Inputs = new FRecipeElement[] {
                    new FRecipeElement(BioInkConfig.DEFAULT, 2),
                    new FRecipeElement(RawEggConfig.ID, 1)
                },
                Outputs = new FRecipeElement[] {
                    new FRecipeElement(BioInkConfig.GERMINATED, 2)
                }
            },

            new FRecipe()
            {
                Description = "PrintingPodRecharge.STRINGS.ITEMS.SEEDED_BIO_INK.DESC",
                Time = 40f,
                Inputs = new FRecipeElement[] {
                    new FRecipeElement(BioInkConfig.DEFAULT, 2),
                    new FRecipeElement(BasicSingleHarvestPlantConfig.SEED_ID, 1)
                },
                Outputs = new FRecipeElement[] {
                    new FRecipeElement(BioInkConfig.SEEDED, 2)
                }
            },

            new FRecipe()
            {
                Description = "PrintingPodRecharge.STRINGS.ITEMS.SEEDED_BIO_INK.DESC",
                Time = 40f,
                Inputs = new FRecipeElement[] {
                    new FRecipeElement(BioInkConfig.DEFAULT, 2),
                    new FRecipeElement(SwampHarvestPlantConfig.SEED_ID, 1)
                },
                Outputs = new FRecipeElement[] {
                    new FRecipeElement(BioInkConfig.SEEDED, 2)
                }
            },

            new FRecipe()
            {
                Description = "PrintingPodRecharge.STRINGS.ITEMS.FOOD_BIO_INK.DESC",
                Time = 40f,
                Inputs = new FRecipeElement[] {
                    new FRecipeElement(BioInkConfig.DEFAULT, 2),
                    new FRecipeElement(MushBarConfig.ID, 1)
                },
                Outputs = new FRecipeElement[] {
                    new FRecipeElement(BioInkConfig.FOOD, 2)
                }
            },

            new FRecipe()
            {
                Description = "PrintingPodRecharge.STRINGS.ITEMS.METALLIC_BIO_INK.DESC",
                Time = 40f,
                Inputs = new FRecipeElement[] {
                    new FRecipeElement(BioInkConfig.DEFAULT, 2),
                    new FRecipeElement(SimHashes.Copper.CreateTag(), 25)
                },
                Outputs = new FRecipeElement[] {
                    new FRecipeElement(BioInkConfig.METALLIC, 2)
                }
            },

            new FRecipe()
            {
                Description = "PrintingPodRecharge.STRINGS.ITEMS.METALLIC_BIO_INK.DESC",
                Time = 40f,
                Inputs = new FRecipeElement[] {
                    new FRecipeElement(BioInkConfig.DEFAULT, 2),
                    new FRecipeElement(SimHashes.Iron.CreateTag(), 25)
                },
                Outputs = new FRecipeElement[] {
                    new FRecipeElement(BioInkConfig.METALLIC, 2)
                }
            },

            new FRecipe()
            {
                Description = "PrintingPodRecharge.STRINGS.ITEMS.VACILLATING_BIO_INK.DESC",
                Time = 40f,
                Inputs = new FRecipeElement[] {
                    new FRecipeElement(BioInkConfig.DEFAULT, 2),
                    new FRecipeElement(GeneShufflerRechargeConfig.ID, 1)
                },
                Outputs = new FRecipeElement[] {
                    new FRecipeElement(BioInkConfig.VACILLATING, 2)
                }
            },

            new FRecipe()
            {
                Description = "PrintingPodRecharge.STRINGS.ITEMS.SHAKER_BIO_INK.DESC",
                Time = 40f,
                Inputs = new FRecipeElement[] {
                    new FRecipeElement(BioInkConfig.DEFAULT, 2),
                    new FRecipeElement(SimHashes.SlimeMold.CreateTag(), 20),
                    new FRecipeElement(SimHashes.Water.CreateTag(), 5),
                    new FRecipeElement(MeatConfig.ID, 1),
                },
                Outputs = new FRecipeElement[] {
                    new FRecipeElement(BioInkConfig.SHAKER, 2)
                }
            },

            new FRecipe()
            {
                Description = "PrintingPodRecharge.STRINGS.ITEMS.MEDICINAL_BIO_INK.DESC",
                Time = 40f,
                Inputs = new FRecipeElement[] {
                    new FRecipeElement(BioInkConfig.DEFAULT, 2),
                    new FRecipeElement(BasicBoosterConfig.ID, 1)
                },
                Outputs = new FRecipeElement[] {
                    new FRecipeElement(BioInkConfig.MEDICINAL, 2)
                }
            },
        };

        internal bool Process()
        {
            if(!BioInks.Any(i => i.Outputs[0].ID == BioInkConfig.MEDICINAL))
            {
                BioInks.Add(new FRecipe()
                {
                    Description = "PrintingPodRecharge.STRINGS.ITEMS.MEDICINAL_BIO_INK.DESC",
                    Time = 40f,
                    Inputs = new FRecipeElement[] {
                    new FRecipeElement(BioInkConfig.DEFAULT, 2),
                    new FRecipeElement(BasicBoosterConfig.ID, 1)
                    },
                        Outputs = new FRecipeElement[] {
                        new FRecipeElement(BioInkConfig.MEDICINAL, 2)
                    }
                });

                return true;
            }

            return false;
        }
    }
}