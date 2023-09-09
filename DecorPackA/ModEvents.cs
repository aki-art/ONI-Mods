namespace DecorPackA
{
	public class ModEvents
	{
		public static ModHashes
			OnMoodlampChanged = new("DecorPackA_OnMoodlampChanged"),
			// triggers for all states, including default
			OnArtableStageSet = new("DecorPackA_OnArtableStageSet"),
			OnLampTinted = new("DecorPackA_OnLampTinted"),
			OnLampRefresh = new("DecorPackA_Moodlamp_RefreshAnimation");
	}
}
