/*using Database;
using FUtility;
using static STRINGS.DUPLICANTS;

namespace PrintingPodRecharge.Content
{
    public class PPersonalities
    {
        public static Personality Shook;
        public const string SHOOK_ID = "BioInks_Shook";

        public static void Register(Personalities personalities)
        {
            foreach(var personality in personalities.resources)
            {
                Log.Debuglog($"personality type: {personality.Id}, {personality.personalityType}");
            }

            Shook = personalities.Add(new Personality(
                SHOOK_ID,
                SHOOK_ID,
                "Male",
                "Grumpy",
                "UglyCrier",
                "BalloonArtist",
                "",
                "",
                1,
                1,
                -1,
                1,
                1,
                new Tag(SHOOK_ID).GetHash(),
                "",
                true)
            {
                Disabled = true // do not naturally print
            });
        }

        public static void AddPersonality(string ID, string gender, string personalityType, string stress, string joy, string sticker, int head, int mouth, int eyes, int hair)
        {
            Db.Get().Personalities.Add(new Personality(
                ID,
                ID,
                gender,
                personalityType,
                stress,
                joy,
                "",
                "",
                head,
                mouth,
                -1,
                eyes,
                hair,
                new Tag(ID).GetHash(),
                "",
                true)
            {
                Disabled = true // do not naturally print
            });
        }
    }
}
*/