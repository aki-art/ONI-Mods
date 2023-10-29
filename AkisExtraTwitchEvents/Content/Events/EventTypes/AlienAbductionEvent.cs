using ONITwitchLib;
using Twitchery.Content.Scripts.UI;

namespace Twitchery.Content.Events.EventTypes
{
	public class AlienAbductionEvent() : TwitchEventBase("AlienAbduction")
	{
		public AdventureScreen screen;

		public override Danger GetDanger() => Danger.Small;

		public override int GetWeight() => WEIGHTS.RARE;

		public override void Run()
		{
			if (screen == null)
				screen = FUtility.FUI.Helper.CreateFDialog<AdventureScreen>(ModAssets.Prefabs.adventureScreen);
			else
			{
				screen.gameObject.SetActive(true);
				screen.ShowDialog();
			}
		}
	}
}
