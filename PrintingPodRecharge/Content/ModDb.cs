using FUtility;
using PrintingPodRecharge.Content.Cmps;
using PrintingPodRecharge.Content.Items;
using System.Collections.Generic;
using System.Linq;
using TUNING;

namespace PrintingPodRecharge.Content
{
	public class ModDb
	{
		public static List<string> dupeSkillIds;

		private static RecipeBuilder StandardRecipe(string description)
		{
			return RecipeBuilder.Create(CraftingTableConfig.ID, description, 40f)
				.NameDisplay(ComplexRecipe.RecipeNameDisplay.Result);
		}

		public static void ConfigureRecipes()
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

			Log.Debug("adding recipes");
			if (ImmigrationModifier.AreBionicDupesEnabled())
				StandardRecipe(STRINGS.ITEMS.BIONIC_BIO_INK.DESC)
					.Input(BioInkConfig.DEFAULT, 2)
					.Input(DatabankHelper.TAG, 1)
					.Output(BioInkConfig.BIONIC, 2)
					.Build();
		}

		public static void OnDbInit(Db _)
		{
			var attributes = Db.Get().Attributes;
			dupeSkillIds =
			[
				attributes.Athletics.Id,
				attributes.Strength.Id,
				attributes.Digging.Id,
				attributes.Construction.Id,
				attributes.Art.Id,
				attributes.Caring.Id,
				attributes.Learning.Id,
				attributes.Machinery.Id,
				attributes.Cooking.Id,
				attributes.Botanist.Id,
				attributes.Ranching.Id
			];

			if (DlcManager.FeatureRadiationEnabled())
				dupeSkillIds.Add(attributes.SpaceNavigation.Id);

			if (Mod.otherMods.IsBeachedHere)
				dupeSkillIds.Add("Beached_Mineralogy");

			// gene shuffler traits were marked as negative for some reason. Possibly an oversight.
			foreach (var trait in DUPLICANTSTATS.GENESHUFFLERTRAITS)
				Db.Get().traits.Get(trait.id).PositiveTrait = true;

			ModAssets.goodTraits = DUPLICANTSTATS.GOODTRAITS.Select(t => t.id).ToHashSet();
			ModAssets.badTraits = DUPLICANTSTATS.BADTRAITS.Select(t => t.id).ToHashSet();
			ModAssets.needTraits = DUPLICANTSTATS.NEEDTRAITS.Select(t => t.id).ToHashSet();
			ModAssets.vacillatorTraits = DUPLICANTSTATS.GENESHUFFLERTRAITS.Select(t => t.id).ToHashSet();

			Integration.TwitchIntegration.DbInit.OnDbInit();

			ModAssets.LateLoadAssets();

			ConfigureRecipes();
		}
	}
}
