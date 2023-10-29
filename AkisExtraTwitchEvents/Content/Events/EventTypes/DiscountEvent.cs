using System;

namespace Twitchery.Content.Events.EventTypes
{
    public class DiscountEvent : ITwitchEvent
    {
        public const string ID = "Discount";
        public int GetWeight() => TwitchEvents.Weights.COMMON;

        public bool Condition(object data) => true;

        public string GetID() => ID;

        public void Run(object data)
        {

        }
    }
}
