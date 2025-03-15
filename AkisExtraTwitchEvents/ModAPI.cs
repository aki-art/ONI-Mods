using System.Collections.Generic;

namespace Twitchery
{
	public class ModAPI
	{
		public static Dictionary<Tag, (Tag tag, float mass)> cookedDropsLookup = new()
		{
			{ MeatConfig.ID, (CookedMeatConfig.ID, 1f) },
			{ FishMeatConfig.ID, (CookedFishConfig.ID, 1f) },
			{ RawEggConfig.ID, (CookedEggConfig.ID, 1f) },
			{ ShellfishMeatConfig.ID, (CookedFishConfig.ID, 1f) },
			{ MushBarConfig.ID, (FriedMushBarConfig.ID, 1f) },
			{ MushroomConfig.ID, (FriedMushroomConfig.ID, 1f) },
			{ PrickleFruitConfig.ID, (GrilledPrickleFruitConfig.ID, 1f) },

			{ WormBasicFruitConfig.ID, (WormBasicFoodConfig.ID, 1f) },

			{ HardSkinBerryConfig.ID, (CookedPikeappleConfig.ID, 1f) }
		};

		public static void AddButcherableCookedDropLookup(Tag drop, Tag cookedMeat, float mass)
		{
			cookedDropsLookup[drop] = (cookedMeat, mass);
		}
	}
}
