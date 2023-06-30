using Klei.AI;
using static STRINGS.DUPLICANTS.MODIFIERS;

namespace Twitchery.Content
{
	internal class TDb
	{
		public static TPolymorphs polymorphs;

		public const string WET = "SoakingWet";
		public const string WETFEET = "WetFeet";

		public static Effect wet;
		public static Effect wetFeet;

		public static void Init(Db db)
		{
			polymorphs = new TPolymorphs();

			wet = db.effects.Get(WET);
			wetFeet = db.effects.Get(WETFEET);
		}
	}
}
