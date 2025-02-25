namespace Twitchery.Content.Scripts
{
	public class AETE_SaveMod : KMonoBehaviour
	{
		public static AETE_SaveMod Instance { get; set; }

		[MyCmpReq] public SolarStormManager solarStorm;
	}
}
