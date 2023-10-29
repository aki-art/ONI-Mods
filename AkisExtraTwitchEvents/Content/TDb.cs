using HarmonyLib;
using Klei.AI;
using System.Collections.Generic;
using System.Linq;

namespace Twitchery.Content
{
	internal class TDb
	{
		public static TPolymorphs polymorphs;
		public static StoryCards storyCards;
		public static Adventures adventures;

		public const string WET = "SoakingWet";
		public const string WETFEET = "WetFeet";

		public static Effect wet;
		public static Effect wetFeet;

		public static Dictionary<Tag, string> beverages = new Dictionary<Tag, string>()
		{
			{ Elements.Honey.Tag, TEffects.HONEY }
		};

		public static void Init(Db db)
		{
			TAccessories.Register(db.Accessories, db.AccessorySlots);
			TTraits.Register();
			TPersonalities.Register(db.Personalities);

			polymorphs = new();
			storyCards = new();
			adventures = new();

			wet = db.effects.Get(WET);
			wetFeet = db.effects.Get(WETFEET);

			var beverages = new List<Tuple<Tag, string>>();

			foreach (var beverage in TDb.beverages)
				beverages.Add(new Tuple<Tag, string>(beverage.Key, beverage.Value));

			WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS = WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS
				.AddRangeToArray(beverages.ToArray())
				.ToArray();
		}
	}
}
