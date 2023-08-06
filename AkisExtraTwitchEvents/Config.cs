using PeterHan.PLib.Options;

namespace Twitchery
{
	[ConfigFile(IndentOutput: true, SharedConfigLocation: true)]
	public class Config
	{
		[Option(
			 "Twitchery.STRINGS.AETE_CONFIG.DOUBLE_TROUBLE.MAX_DUPES.LABEL",
			 "Twitchery.STRINGS.AETE_CONFIG.DOUBLE_TROUBLE.MAX_DUPES.TOOLTIP",
			 "Twitchery.STRINGS.AETE_EVENTS.DOUBLE_TROUBLE.TOAST")]
		public int MaxDupes { get; set; }

		public int Version { get; set; }

		public Config()
		{
			MaxDupes = 40;
		}
	}
}
