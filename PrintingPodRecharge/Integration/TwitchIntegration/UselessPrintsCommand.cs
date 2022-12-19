#if TWITCH
using PrintingPodRecharge.Cmps;

namespace PrintingPodRecharge.Integration.TwitchIntegration
{
    public class UselessPrintsCommand
    {
        public const string ID = "UselessPrints";
        public static bool queued;

        public static bool Condition() => true;

        public static void Run(object data)
        {
            if(Immigration.Instance.ImmigrantsAvailable)
            {
                queued = true;

                ONITwitchLib.ToastManager.InstantiateToast("Useless prints are ready!", "Useless prints have been queued.");
                return;
            }

            Print();
        }

        public static void Print()
        {
            ImmigrationModifier.Instance.SetModifier(Bundle.Twitch);
            Immigration.Instance.timeBeforeSpawn = 0;

            // ONITwitchLib.ToastManager.InstantiateToastWithGoTarget("Useless prings are ready!", "New printables are ready to print.", GameUtil.GetActiveTelepad());
            queued = false;
        }
    }
}
#endif