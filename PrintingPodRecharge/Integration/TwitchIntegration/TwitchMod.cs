using ONITwitchLib;
using ONITwitchLib.Core;

namespace PrintingPodRecharge.Integration.TwitchIntegration
{
	public class TwitchMod
	{
		public static void OnDbInit()
		{
			if (!TwitchModInfo.TwitchIsPresent)
				return;

			var deckInst = TwitchDeckManager.Instance;

			var (leakEvent, leakGroup) = EventGroup.DefaultSingleEventGroup(PrintingPodLeakCommand.ID, 30, STRINGS.TWITCH.PRINTING_POD_LEAK.NAME);
			leakEvent.AddListener(PrintingPodLeakCommand.Run);
			leakEvent.AddCondition(PrintingPodLeakCommand.Condition);
			leakEvent.Danger = Danger.None;
			deckInst.AddGroup(leakGroup);

			var (helpfulEvent, helpfulGroup) = EventGroup.DefaultSingleEventGroup(HelpfulPrintsCommand.ID, 30, STRINGS.TWITCH.HELPFUL_PRINTS.NAME);
			helpfulEvent.AddListener(HelpfulPrintsCommand.Run);
			helpfulEvent.Danger = Danger.None;
			deckInst.AddGroup(helpfulGroup);

			var (uselessEvent, uselessEventGroup) = EventGroup.DefaultSingleEventGroup(UselessPrintsCommand.ID, 30, STRINGS.TWITCH.USELESS_PRINTS.NAME);
			uselessEvent.AddListener(UselessPrintsCommand.Run);
			uselessEvent.Danger = Danger.None;
			deckInst.AddGroup(uselessEventGroup);

			/*
			var (wackyEvent, wackyEventEventGroup) = EventGroup.DefaultSingleEventGroup(WackyDupeCommand.ID, 15, STRINGS.TWITCH.USELESS_PRINTS.NAME);
			uselessEvent.AddListener(WackyDupeCommand.Run);
			uselessEvent.Danger = Danger.Small;
			deckInst.AddGroup(wackyEventEventGroup);*/
		}
	}
}