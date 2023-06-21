using FUtility;
using KSerialization;
using UnityEngine;

namespace TwitchEventsTimer
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class TwitchEventsTimer_Mod : KMonoBehaviour, ISim200ms
	{
		[Serialize] public float elapsedSinceLastEventSeconds;
		[Serialize] public float nextEventInSeconds;
		[Serialize] public bool paused;

		public TwitchEventsTimer_Mod() => nextEventInSeconds = -1;

		protected override void OnSpawn()
		{
			base.OnSpawn();

			if (nextEventInSeconds == -1)
				SetupNextEvent();
		}

		private void SetupNextEvent()
		{
			elapsedSinceLastEventSeconds = 0;
			nextEventInSeconds = Random.Range(
				Mod.Settings.CyclesBetweenEventsMin,
				Mod.Settings.CyclesBetweenEventsMax) * Consts.CYCLE_LENGTH;
		}

		public void Sim200ms(float dt)
		{
			if (paused)
				return;

			elapsedSinceLastEventSeconds += dt;

			if (elapsedSinceLastEventSeconds >= nextEventInSeconds)
				TriggerEvent();
		}

		private void TriggerEvent()
		{
			var ev = ONITwitchLib.Core.TwitchDeckManager.Instance.Draw();
			var data = ONITwitchLib.DataManager.Instance.GetDataForEvent(ev);
			ev.Trigger(data);

			SetupNextEvent();
		}

		private void OnGUI()
		{
			if (Mod.Settings.ShowUI)
			{
				GUILayout.BeginArea(new Rect(20, 110, 200, 30));
				GUILayout.Label("Next event in " + GameUtil.GetFormattedTime(nextEventInSeconds - elapsedSinceLastEventSeconds));
				GUILayout.EndArea();
			}
		}
	}
}
