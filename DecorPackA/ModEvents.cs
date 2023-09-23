namespace DecorPackA
{
	public class ModEvents
	{
		public static ModHashes
			OnMoodlampChanged = new("DecorPackA_OnMoodlampChanged"),
			OnMoodlampChangedEarly = new("DecorPackA_OnMoodlampChangedEarly"),
			// triggers for all states, including default
			OnArtableStageSet = new("DecorPackA_OnArtableStageSet"),
			OnLampTinted = new("DecorPackA_OnLampTinted"),
			// runs after animation has been refreshed with default logic
			// still triggered if the refresh was skipped with the SKIP_ANIMATION_UPDATE tag
			OnLampRefreshedAnimation = new("DecorPackA_Moodlamp_RefreshedAnimation");
	}
}
