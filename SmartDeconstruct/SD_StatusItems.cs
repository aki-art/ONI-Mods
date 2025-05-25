using Database;

namespace SmartDeconstruct
{
	class SD_StatusItems
	{
		public static StatusItem queuedForDeconstruction;


		public static void Register(MiscStatusItems parent)
		{
			queuedForDeconstruction = parent.Add(new StatusItem(
				"SmartDeconstruct_QueuedForDeconstruction",
				"MISC",
				"action_deconstruct",
				StatusItem.IconType.Info,
				NotificationType.Neutral,
				false,
				OverlayModes.None.ID));
		}
	}
}
