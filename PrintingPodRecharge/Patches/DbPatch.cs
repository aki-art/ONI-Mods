using FUtility;
using HarmonyLib;
using PrintingPodRecharge.Items;
using TUNING;

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
                // gene shuffler traits were marked as negative for some reason. Possibly an oversight.
                foreach (var trait in DUPLICANTSTATS.GENESHUFFLERTRAITS)
                {
                    Db.Get().traits.Get(trait.id).PositiveTrait = true;
                }

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
