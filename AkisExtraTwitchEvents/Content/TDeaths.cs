using Database;

namespace Twitchery.Content
{
	public class TDeaths
	{
		public static Death lasered;

		public static void Register(Deaths deaths)
		{
			lasered = new Death(
				"AETE_Death_Lasered",
				deaths,
				STRINGS.DEATHS.AETE_LASERED.NAME,
				STRINGS.DEATHS.AETE_LASERED.DESCRIPTION,
				"death_suffocation",
				"dead_on_back");
		}
	}
}
