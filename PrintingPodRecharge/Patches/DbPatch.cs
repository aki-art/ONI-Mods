using FUtility;
using HarmonyLib;
using PrintingPodRecharge.Items;

namespace PrintingPodRecharge.Patches
{
    public class DbPatch
    {
        [HarmonyPatch(typeof(Db), "Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                ModAssets.LateLoadAssets();
            }

            public static void Postfix()
            {
                RecipeBuilder.Create(CraftingTableConfig.ID, STRINGS.ITEMS.GERMINATED_BIO_INK.DESC, 40f)
                    .Input(BioInkConfig.DEFAULT, 2f)
                    .Input(RawEggConfig.ID, 1f)
                    .Output(BioInkConfig.GERMINATED, 2f)
                    .Build();

                RecipeBuilder.Create(CraftingTableConfig.ID, STRINGS.ITEMS.METALLIC_BIO_INK.DESC, 40f)
                    .Input(BioInkConfig.DEFAULT, 2f)
                    .Input(SimHashes.Iron.CreateTag(), 25f)
                    .Output(BioInkConfig.METALLIC, 2f)
                    .Build();

                RecipeBuilder.Create(CraftingTableConfig.ID, STRINGS.ITEMS.METALLIC_BIO_INK.DESC, 40f)
                    .Input(BioInkConfig.DEFAULT, 2f)
                    .Input(SimHashes.Copper.CreateTag(), 25f)
                    .Output(BioInkConfig.METALLIC, 2f)
                    .Build();

                RecipeBuilder.Create(CraftingTableConfig.ID, STRINGS.ITEMS.OOZING_BIO_INK.DESC, 40f)
                    .Input(BioInkConfig.DEFAULT, 2f)
                    .Input(SimHashes.Creature.CreateTag(), 25f)
                    .Output(BioInkConfig.OOZING, 2f)
                    .Build();

                RecipeBuilder.Create(CraftingTableConfig.ID, STRINGS.ITEMS.VACILLATING_BIO_INK.DESC, 40f)
                    .Input(BioInkConfig.DEFAULT, 2f)
                    .Input(SimHashes.Creature.CreateTag(), 25f)
                    .Input(GeneShufflerRechargeConfig.ID, 1f)
                    .Output(BioInkConfig.VACILLATING, 2f)
                    .Build();
            }
        }
    }
}
