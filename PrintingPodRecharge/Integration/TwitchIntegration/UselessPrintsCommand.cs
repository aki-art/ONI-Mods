using PrintingPodRecharge.Content.Cmps;

namespace PrintingPodRecharge.Integration.TwitchIntegration
{
	public class UselessPrintsCommand
	{
		public const string ID = "UselessPrints";
		public static bool queued;

		public static bool Condition() => true;

		public static void Run(object data)
		{
			if (Immigration.Instance.ImmigrantsAvailable)
			{
				queued = true;

				ONITwitchLib.ToastManager.InstantiateToast(
					STRINGS.TWITCH.USELESS_PRINTS.TOAST,
					STRINGS.TWITCH.USELESS_PRINTS.DESC_QUEUED);

				return;
			}

			Print();
		}

		public static void Print()
		{
			ImmigrationModifier.Instance.SetModifier(Bundle.Twitch);
			Immigration.Instance.timeBeforeSpawn = 0;

			ONITwitchLib.ToastManager.InstantiateToastWithGoTarget(
				STRINGS.TWITCH.USELESS_PRINTS.TOAST,
				STRINGS.TWITCH.HELPFUL_PRINTS.DESC_READY,
				GameUtil.GetActiveTelepad());

			queued = false;
		}
	}
}
