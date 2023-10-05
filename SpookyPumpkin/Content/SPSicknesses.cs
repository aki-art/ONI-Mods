using SpookyPumpkinSO.Content.Sicknesses;

namespace SpookyPumpkinSO.Content
{
	public class SPSicknesses
	{
		public static void Register(Database.Sicknesses sicknesses)
		{
			sicknesses.Add(new SugarSickness());
		}
	}
}
