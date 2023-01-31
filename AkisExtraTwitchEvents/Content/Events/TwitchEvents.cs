using FUtility;
using ONITwitchLib;
using ONITwitchLib.Core;

namespace Twitchery.Content.Events
{
    public class TwitchEvents
    {
        public static ColorScrambleEvent colorScrambleEvent;

        public class Weights
        {
            public const int 
                COMMON = 20,
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
            //deckInst.AddGroup(SetupEvent(new LongWormEvent(), "Long Boi"));
            deckInst.AddGroup(SetupEvent(new JelloRainEvent(), "Jello Rain"));
            deckInst.AddGroup(SetupEvent(new ScreenPipEvent(), "Desktop Lettuce"));
            deckInst.AddGroup(SetupEvent(new RadDishEvent(), "Rad Dish"));
            deckInst.AddGroup(SetupEvent(new PizzaDeliveryEvent(), "Pizza Delivery"));
        }

        private static EventGroup SetupEvent(ITwitchEvent eventInstance, string friendlyName, Danger danger = Danger.None)
        {
            var (ev, group) = EventGroup.DefaultSingleEventGroup(eventInstance.GetID(), Weights.COMMON, friendlyName);
            ev.AddListener(eventInstance.Run);
            ev.AddCondition(eventInstance.Condition);
            ev.Danger = danger;

            return group;
        }
    }
}
