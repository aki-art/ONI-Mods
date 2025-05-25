using Database;

namespace Twitchery.Content
{

	public class TOrbitalTypeCategories
	{
		public static OrbitalData harvestMoon;

		public static void Register(OrbitalTypeCategories orbitalTypeCategories)
		{
			harvestMoon = new OrbitalData("AkisExtraTwitchEvents_HarvestMoon", orbitalTypeCategories, "aete_harvest_moon_kanim", "idle", orbitalType: OrbitalData.OrbitalType.world, yGridPercent: 0.92f, minAngle: 30f, maxAngle: 30f, radiusScale: 1.02f);
		}
	}
}
