using DecorPackA.Integration.Twitch;
using ONITwitchLib;
using ONITwitchLib.Core;

namespace DecorPackA.Integration
{
	public class TwitchMod
	{
		public static void Initialize()
		{
			if (!TwitchModInfo.TwitchIsPresent)
				return;

			var deckInst = TwitchDeckManager.Instance;

			var (floorUpgradeEvent, floorUpgradeGroup) = EventGroup.DefaultSingleEventGroup(
				 FloorUpgradeCommand.ID,
				 33,
				 STRINGS.TWITCH.FLOOR_UPGRADE.NAME);

			floorUpgradeEvent.AddListener(FloorUpgradeCommand.Run);
			floorUpgradeEvent.Danger = Danger.Small;
			deckInst.AddGroup(floorUpgradeGroup);
		}
	}
}
