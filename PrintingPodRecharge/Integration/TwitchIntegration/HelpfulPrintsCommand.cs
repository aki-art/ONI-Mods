using PrintingPodRecharge.Cmps;

namespace PrintingPodRecharge.Integration.TwitchIntegration
{
    public class HelpfulPrintsCommand
    {
        public const string ID = "HelpfulPrints";
        public static bool queued;

        public static bool Condition() => true;

        public static void Run(object data)
        {
            if (Immigration.Instance.ImmigrantsAvailable)
            {
                queued = true;

                ONITwitchLib.ToastManager.InstantiateToast("Helpful prints are ready!", "Helpful prints have been queued.");
                return;
            }

            Print();
        }

        public static void Print()
        {
            ImmigrationModifier.Instance.SetModifier(Bundle.TwitchHelpful);
            Immigration.Instance.timeBeforeSpawn = 0;

            ONITwitchLib.ToastManager.InstantiateToastWithGoTarget("Helpful prints are ready!", "New printables are ready to print.", GameUtil.GetActiveTelepad());
            queued = false;
        }
    }
}
