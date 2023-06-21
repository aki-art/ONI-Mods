using PeterHan.PLib.Options;

namespace TwitchEventsTimer
{
	public class Config
	{
		[Option("Minimum cycles between events")]
		public float CyclesBetweenEventsMin {  get; set; }

		[Option("Maximum cycles between events")]
		public float CyclesBetweenEventsMax {  get; set; }

		[Option("Show UI")]
		public bool ShowUI { get; set; }

		public Config()
		{
			CyclesBetweenEventsMin = 1f;
			CyclesBetweenEventsMax = 1f;
			ShowUI = true;
		}
	}
}
