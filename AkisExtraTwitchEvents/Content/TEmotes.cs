using Database;
using Klei.AI;

namespace Twitchery.Content
{
    public class TEmotes
    {
        public static Emote coffeeBreak;

        public static void Register(Emotes.MinionEmotes emotes)
        {
            coffeeBreak = new Emote(emotes, "AkisExtraTwitchEvents_CoffeeBreak", new[]
            {
                new EmoteStep
                {
                    anim = "working_pre"
                },
                new EmoteStep
                {
                    anim = "working_loop"
                },
                new EmoteStep
                {
                    anim = "working_loop"
                },
                new EmoteStep
                {
                    anim = "working_loop"
                },
                new EmoteStep
                {
                    anim = "working_pst"
                }
            },
            "aete_interacts_espresso_short_kanim");
        }
    }
}
