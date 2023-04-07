﻿using FUtility;
using ONITwitchLib.Core;
using ONITwitchLib;

namespace DecorPackB.Integration.Twitch
{
    public class TwitchEvents
    {
        public static void PostDbInit()
        {
            if (!TwitchModInfo.TwitchIsPresent)
            {
                Log.Debuglog("Twitch not enabled");
                return;
            }

            var deckInst = TwitchDeckManager.Instance;

            var (luckyPotEvent, potGroup) = EventGroup.DefaultSingleEventGroup(LuckyPotsCommand.ID, 30, STRINGS.TWITCH.LUCKY_POTS.NAME);
            luckyPotEvent.AddListener(LuckyPotsCommand.Run);
            luckyPotEvent.AddCondition(LuckyPotsCommand.Condition);
            luckyPotEvent.Danger = Danger.None;

            deckInst.AddGroup(potGroup);
        }
    }
}