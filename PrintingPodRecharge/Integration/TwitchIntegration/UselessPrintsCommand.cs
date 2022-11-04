using PrintingPodRecharge.Cmps;

namespace PrintingPodRecharge.Integration.TwitchIntegration
{
    public class UselessPrintsCommand
    {
        public const string ID = "UselessPrints";
        public static bool queued;

        public static bool Condition() => true;

        public static void Run()
        {
            if(Immigration.Instance.ImmigrantsAvailable)
            {
                queued = true;
                return;
            }

            Print();
        }

        public static void Print()
        {
            ImmigrationModifier.Instance.SetModifier(Bundle.Twitch);
            Immigration.Instance.timeBeforeSpawn = 0;
            queued = false;
        }
    }
}
