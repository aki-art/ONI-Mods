#if TWITCH
using FUtility;
using ONITwitchLib;

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

            var eventInst = EventInterface.GetEventManagerInstance();
            var dataInst = EventInterface.GetDataManagerInstance();
            var conditionsInst = EventInterface.GetConditionsManager();
            var deckInst = EventInterface.GetDeckManager();
            var dangerInst = EventInterface.GetDangerManager();

            var leakEvent = eventInst.RegisterEvent(PrintingPodLeakCommand.ID, STRINGS.TWITCH.PRINTING_POD_LEAK.NAME);
            eventInst.AddListenerForEvent(leakEvent, PrintingPodLeakCommand.Run);
            deckInst.AddToDeck(leakEvent, 30);
            dangerInst.SetDanger(leakEvent, Danger.None);
            conditionsInst.AddCondition(leakEvent, PrintingPodLeakCommand.Condition);

            var helpfulEvent = eventInst.RegisterEvent(HelpfulPrintsCommand.ID, STRINGS.TWITCH.HELPFUL_PRINTS.NAME);
            eventInst.AddListenerForEvent(helpfulEvent, HelpfulPrintsCommand.Run);
            deckInst.AddToDeck(helpfulEvent, 30);
            dangerInst.SetDanger(helpfulEvent, Danger.None);

            var uselessEvent = eventInst.RegisterEvent(UselessPrintsCommand.ID, STRINGS.TWITCH.USELESS_PRINTS.NAME);
            eventInst.AddListenerForEvent(uselessEvent, UselessPrintsCommand.Run);
            deckInst.AddToDeck(uselessEvent, 30);
            dangerInst.SetDanger(uselessEvent, Danger.None);

/*            var wackyEvent = eventInst.RegisterEvent(WackyDupeCommand.ID, STRINGS.TWITCH.WACKY_DUPE.NAME);
            eventInst.AddListenerForEvent(wackyEvent, WackyDupeCommand.Run);
            deckInst.AddToDeck(wackyEvent, 15);
            dangerInst.SetDanger(wackyEvent, Danger.Small);*/

            var floorUpgradeEvent = eventInst.RegisterEvent(FloorUpgradeCommand.ID, STRINGS.TWITCH.FLOOR_UPGRADE.NAME);
            eventInst.AddListenerForEvent(floorUpgradeEvent, FloorUpgradeCommand.Run);
            deckInst.AddToDeck(floorUpgradeEvent, 33);
            dangerInst.SetDanger(floorUpgradeEvent, Danger.Small);
        }
    }
}
#endif