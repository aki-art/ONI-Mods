#if TWITCH
using FUtility;
using ONITwitchLib;
using ONITwitchLib.Core;

namespace PrintingPodRecharge.Integration.TwitchIntegration
{
    public class TwitchMod
    {
        public static void OnAllModsLoaded()
        {
            if (!TwitchModInfo.TwitchIsPresent)
            {
                Log.Debuglog("Twitch not enabled");
                return;
            }

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

/*            var wackyEvent = eventInst.RegisterEvent(WackyDupeCommand.ID, STRINGS.TWITCH.WACKY_DUPE.NAME);
            eventInst.AddListenerForEvent(wackyEvent, WackyDupeCommand.Run);
            deckInst.AddToDeck(wackyEvent, 15);
            dangerInst.SetDanger(wackyEvent, Danger.Small);*/

            var (floorUpgradeEvent, floorUpgradeGroup) = EventGroup.DefaultSingleEventGroup(FloorUpgradeCommand.ID, 33, STRINGS.TWITCH.FLOOR_UPGRADE.NAME);
            floorUpgradeEvent.AddListener(FloorUpgradeCommand.Run);
            floorUpgradeEvent.Danger = Danger.Small;
            deckInst.AddGroup(floorUpgradeGroup);
        }
    }
}
#endif