using FUtility;
using HarmonyLib;
using PrintingPodRecharge.Content.Items;

namespace PrintingPodRecharge.Patches
{
	public class EntityConfigManagerPatch
	{
		[HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
		public class EntityConfigManager_LoadGeneratedEntities_Patch
		{
			private static RecipeBuilder StandardRecipe(string description)
			{
				return RecipeBuilder.Create(CraftingTableConfig.ID, description, 40f)
					.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result);
			}

			public static void Postfix()
			{
				StandardRecipe(STRINGS.ITEMS.GERMINATED_BIO_INK.DESC)
					.Input(BioInkConfig.DEFAULT, 2)
					.Input(RawEggConfig.ID, 1)
					.Output(BioInkConfig.GERMINATED, 2)
					.Build();

				StandardRecipe(STRINGS.ITEMS.SEEDED_BIO_INK.DESC)
					.Input(BioInkConfig.DEFAULT, 2)
					.Input([
							BasicSingleHarvestPlantConfig.SEED_ID,
							SwampHarvestPlantConfig.SEED_ID,
							"Beached_AlgaeCellSeed",
							"Beached_CapCapSeed",
							GardenFoodPlantConfig.SEED_ID,
							HardSkinBerryPlantConfig.ID
						], 1)
					.Output(BioInkConfig.SEEDED, 2)
					.Build();

				StandardRecipe(STRINGS.ITEMS.FOOD_BIO_INK.DESC)
					.Input(BioInkConfig.DEFAULT, 2)
					.Input(MushBarConfig.ID, 1)
					.Output(BioInkConfig.FOOD, 2)
					.Build();

				StandardRecipe(STRINGS.ITEMS.METALLIC_BIO_INK.DESC)
					.Input(BioInkConfig.DEFAULT, 2)
					.Input(GameTags.BasicRefinedMetals, 25)
					.Output(BioInkConfig.METALLIC, 2)
					.Build();

				StandardRecipe(STRINGS.ITEMS.VACILLATING_BIO_INK.DESC)
					.Input(BioInkConfig.DEFAULT, 2)
					.Input(GeneShufflerRechargeConfig.ID, 1)
					.Output(BioInkConfig.VACILLATING, 2)
					.Build();

				StandardRecipe(STRINGS.ITEMS.SHAKER_BIO_INK.DESC)
					.Input(SimHashes.SlimeMold.CreateTag(), 20)
					.Input(SimHashes.Water.CreateTag(), 5)
					.Input(MeatConfig.ID, 1)
					.Output(BioInkConfig.SHAKER, 2)
					.Build();

				if (Mod.otherMods.IsDiseasesExpandedHere)
				{
					StandardRecipe(STRINGS.ITEMS.MEDICINAL_BIO_INK.DESC)
						.Input(BioInkConfig.DEFAULT, 2)
						.Input(BasicBoosterConfig.ID, 1)
						.Output(BioInkConfig.GERMINATED, 2)
						.Build();
				}

				StandardRecipe(STRINGS.ITEMS.BIONIC_BIO_INK.DESC)
					.Input(BioInkConfig.DEFAULT, 2)
					.Input(DatabankHelper.TAG, 1)
					.Output(BioInkConfig.BIONIC, 2)
					.Build();
			}
		}
	}
}
