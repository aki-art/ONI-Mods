using FUtility;
using ONITwitchLib;
using ONITwitchLib.Core;
using System.Collections.Generic;

namespace Twitchery.Content.Events
{
    public class TwitchEvents
    {
        public static ColorScrambleEvent colorScrambleEvent;

        public static List<ITwitchEvent> myEvents = new();
        public class Weights
        {
            public const int 
                COMMON = 25,
                RARE = 10,
                GUARANTEED = 20000;
        }

        public static void OnDbInit()
        {
            if (!TwitchModInfo.TwitchIsPresent)
            {
                Log.Debuglog("Twitch not enabled");
                return;
            }

            var deckInst = TwitchDeckManager.Instance;

            //deckInst.AddGroup(SetupEvent(new ColorScrambleEvent(), "Scrambled colors"));
            deckInst.AddGroup(SetupEvent(new CoffeeBreakEvent(), "Coffee Break"));
            // unfinished deckInst.AddGroup(SetupEvent(new MidasTouchEvent(), "Midas Time", Danger.Small));
           // deckInst.AddGroup(SetupEvent(new LongWormEvent(), "Long Boi"));
            deckInst.AddGroup(SetupEvent(new JelloRainEvent(), "Jello Rain"));
            //deckInst.AddGroup(SetupEvent(new ScreenPipEvent(), "Desktop Lettuce"));
            deckInst.AddGroup(SetupEvent(new RadDishEvent(), "Rad Dish"));
            deckInst.AddGroup(SetupEvent(new PizzaDeliveryEvent(), "Pizza Delivery"));
            deckInst.AddGroup(SetupEvent(new RetroVisionEvent(), "Retro Vision"));
            deckInst.AddGroup(SetupEvent<DoubleTroubleEvent>("Double Trouble"));
            deckInst.AddGroup(SetupEvent<InvisibleLiquidsEvent>("Invisible Liquids"));
        }

        private static EventGroup SetupEvent<T>(string friendlyName, Danger danger = Danger.None) where T : ITwitchEvent, new()
        {
            ITwitchEvent eventInstance = new T();
            var (ev, group) = EventGroup.DefaultSingleEventGroup(eventInstance.GetID(), Weights.COMMON, friendlyName);
            ev.AddListener(eventInstance.Run);
            ev.AddCondition(eventInstance.Condition);
            ev.Danger = danger;

            myEvents.Add(eventInstance);

            return group;
        }

        private static EventGroup SetupEvent(ITwitchEvent eventInstance, string friendlyName, Danger danger = Danger.None)
        {
            var (ev, group) = EventGroup.DefaultSingleEventGroup(eventInstance.GetID(), Weights.COMMON, friendlyName);
            ev.AddListener(eventInstance.Run);
            ev.AddCondition(eventInstance.Condition);
            ev.Danger = danger;

            myEvents.Add(eventInstance);

            return group;
        }
    }
}
