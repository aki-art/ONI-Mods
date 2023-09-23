using FUtility;
using static DecorPackA_ExampleAddon.DecorPackA_ModAPI;

namespace DecorPackA_ExampleAddon
{
	public class TestMoodlampBehavior : KMonoBehaviour
	{
		protected override void OnSpawn()
		{
			base.OnSpawn();
			Subscribe(MoodlampChangedEvent, OnMoodlampChanged);
		}

		private void OnMoodlampChanged(object obj)
		{
			if(TryGetData<string>(obj, Keys.Id, out var id))
			{
				Log.Debuglog("received data: " + id);
			}
		}
	}
}
