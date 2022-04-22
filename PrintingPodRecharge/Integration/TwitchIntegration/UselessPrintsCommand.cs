using PrintingPodRecharge.Cmps;

namespace PrintingPodRecharge.Integration.TwitchIntegration
{
    public class UselessPrintsCommand
    {
        public const string ID = "UselessPrints";

        public static bool Condition() => true;

        public static void Run(int danger)
        {
            Immigration.Instance.EndImmigration();
            ImmigrationModifier.Instance.SetModifier(ImmigrationModifier.Bundle.Twitch);
            Immigration.Instance.timeBeforeSpawn = 0;
        }
    }
}
