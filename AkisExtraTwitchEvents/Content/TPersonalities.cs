using Database;
using FUtility;
using System.Collections.Generic;
using System.Linq;

namespace Twitchery.Content
{
	public class TPersonalities
	{
		public const string HULK = "AKISEXTRATWITCHEVENTS_HULK";
		public static readonly int HULK_HASH = Hash.SDBMLower(HULK); // -298189437

		public static Dictionary<HashedString, string> headKanims = new();

		public static void Register(Personalities personalities)
		{
			Log.Debug("registering personalities");

			var hulk = new Personality(
				HULK,
				STRINGS.DUPLICANTS.PERSONALITIES.AKISEXTRATWITCHEVENTS_HULK.NAME,
				"Male",
				null,
				"Aggressive",
				"BalloonArtist",
				"",
				null,
				1,
				1,
				HULK_HASH,
				6,
				7,
				HULK_HASH,
				HULK_HASH,
				HULK_HASH,
				HULK_HASH,
				HULK_HASH,
				HULK_HASH,
				HULK_HASH,
				STRINGS.DUPLICANTS.PERSONALITIES.AKISEXTRATWITCHEVENTS_HULK.DESC,
				false);

			hulk.Disabled = true; // do not show up in menus and such

			personalities.Add(hulk);

			headKanims[(HashedString)HULK] = "aete_hulk_head_kanim";
		}

		public static void ModifyBodyData(Personality p, ref KCompBuilder.BodyData bodyData)
		{
			if (p.Id == HULK)
			{
				bodyData.headShape = HashCache.Get().Add("headshape_hulk");
				bodyData.mouth = HashCache.Get().Add("mouth_hulk");
			}
		}

		public static void OnTraitRoll(MinionStartingStats stats)
		{
			if (stats.personality.Id == HULK)
			{
				stats.Traits.Add(Db.Get().traits.Get(TTraits.ANGRY));
				if (!stats.Traits.Any(t => t.Id == "SlowLearner"))
					stats.Traits.Add(Db.Get().traits.Get("SlowLearner"));
			}
		}
	}
}
