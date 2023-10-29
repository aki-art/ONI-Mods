using FUtility;

namespace Twitchery
{
	public class ModEvents
	{
		public static ModHashes
			OnScreenResize = new("AkisExtraTwitchEvents_OnScreenResize"),
			FailedToFindSafety = new("AkisExtraTwitchEvents_FailedToFindSafety"),
			FoundSafety = new("AkisExtraTwitchEvents_FoundSafety"),
			PocketDimensionClosing = new("AkisExtraTwitchEvents_PocketDimensionClosing"),
			OnHighlightApplied = new("AkisExtraTwitchEvents_OnHighlightApplied"),
			OnHighlightCleared = new("AkisExtraTwitchEvents_OnHighlightCleared");
	}
}
