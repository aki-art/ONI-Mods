using Database;
using FUtility;
using System.Collections.Generic;

namespace Twitchery.Content
{
	public class TPersonalities
	{
		public const string HULK = "AKISEXTRATWITCHEVENTS_HULK";
		public static readonly int HULK_HASH = Hash.SDBMLower(HULK); // -298189437

		public static Dictionary<HashedString, string> headKanims = new();

		public static void Register(Personalities personalities)
		{
			Log.Debuglog("registering personalities");

			personalities.Add(new Personality(
					HULK,
					STRINGS.DUPLICANTS.PERSONALITIES.AKISEXTRATWITCHEVENTS_HULK.NAME,
					"NB",
					null,
					"UglyCrier",
					"SparkleStreaker",
					"",
					null,
					1,
					1,
					1,
					6,
					7,
					HULK_HASH,
					1,
					1,
					1,
					1,
					1,
					1,
					STRINGS.DUPLICANTS.PERSONALITIES.AKISEXTRATWITCHEVENTS_HULK.DESC,
					false));

			headKanims[(HashedString)HULK] = "aete_hulk_head_kanim";
		}

		public static void ModifyBodyData(Personality p, ref KCompBuilder.BodyData bodyData)
		{
			if (p.Id == HULK)
			{
				bodyData.headShape = HashCache.Get().Add("headshape_hulk");
				bodyData.mouth = HashCache.Get().Add("mouth_hulk");
				//bodyData.eyes = HashCache.Get().Add("eyes_hulk");
			}
		}
	}
}
