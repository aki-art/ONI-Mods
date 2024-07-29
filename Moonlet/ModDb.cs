using Klei.AI;
using System.Collections.Generic;

namespace Moonlet
{
	public class ModDb
	{
		public const string WET = "SoakingWet";
		public const string WETFEET = "WetFeet";

		public static Effect wet;
		public static Effect wetFeet;

		public static Dictionary<string, HashSet<Tag>> effectTags;

		public static void OnModInitialize()
		{
			effectTags = new Dictionary<string, HashSet<Tag>>()
			{
				{ WETFEET, [ModTags.EffectTags.WetFeet] },
				{ WET, [ModTags.EffectTags.Soaked] }
			};
		}

		public static void OnDbInitialize(Db db)
		{
			wet = db.effects.Get(WET);
			wetFeet = db.effects.Get(WETFEET);

		}
	}
}
