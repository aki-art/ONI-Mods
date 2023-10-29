using Database;
using Twitchery.Content.Defs.Foods;
using Twitchery.Content.Scripts;

namespace Twitchery.Content
{
	public class TStatusItems
	{
		public static StatusItem CalorieStatus;
		public static StatusItem PolymorphStatus;
		public static StatusItem GoldStruckStatus;
		public static StatusItem FrozenStatus;
		public static StatusItem PuzzleDoorStatus;

		public static void Register(MiscStatusItems parent)
		{
			PolymorphStatus = parent.Add(new StatusItem(
				"AkisExtraTwitchEvents_PolymorphStatus",
				"MISC",
				"status_item_doubleexclamation",
				StatusItem.IconType.Info,
				NotificationType.Neutral,
				false,
				OverlayModes.None.ID));

			PolymorphStatus.SetResolveStringCallback((str, data) =>
			{
				return data is AETE_PolymorphCritter polymorph
					? string.Format(
						str,
						polymorph.GetProperName(),
						polymorph.originalSpeciesname,
						GameUtil.GetFormattedTime(polymorph.duration - polymorph.elapsedTime))
					: str;
			});

			CalorieStatus = parent.Add(new StatusItem(
				"AkisExtraTwitchEvents_CalorieStatus",
				"MISC",
				"status_item_doubleexclamation",
				StatusItem.IconType.Info,
				NotificationType.Neutral,
				false,
				OverlayModes.None.ID));

			CalorieStatus.SetResolveStringCallback((str, data) =>
			{
				if (data is Radish radish)
				{
					var amount = radish.raddishStorage.MassStored();
					return string.Format(str, GameUtil.GetFormattedCaloriesForItem(RawRadishConfig.ID, amount));
				}

				return str;
			});

			GoldStruckStatus = parent.Add(new StatusItem(
				"AkisExtraTwitchEvents_GoldStruckStatus",
				"MISC",
				"status_item_doubleexclamation",
				StatusItem.IconType.Exclamation,
				NotificationType.BadMinor,
				false,
				OverlayModes.None.ID));

			GoldStruckStatus.SetResolveStringCallback(MidasString);

			FrozenStatus = parent.Add(new StatusItem(
				"AkisExtraTwitchEvents_FrozenStatus",
				"MISC",
				"status_item_doubleexclamation",
				StatusItem.IconType.Exclamation,
				NotificationType.BadMinor,
				false,
				OverlayModes.None.ID));

			FrozenStatus.SetResolveStringCallback(MidasString);

			PuzzleDoorStatus = parent.Add(new StatusItem(
				"AkisExtraTwitchEvents_PuzzleDoorStatus",
				"MISC",
				"",
				StatusItem.IconType.Info,
				NotificationType.Neutral,
				false,
				OverlayModes.None.ID));

			PuzzleDoorStatus.SetResolveStringCallback(PuzzleDoorString);
		}

		private static string PuzzleDoorString(string str, object data)
		{
			if (data is PuzzleDoor2 door)
			{
				return door.isSolved
					? global::STRINGS.BUILDING.STATUSITEMS.CURRENTDOORCONTROLSTATE.OPENED
					: global::STRINGS.BUILDING.STATUSITEMS.CURRENTDOORCONTROLSTATE.LOCKED;
			}

			return str;
		}

		private static string MidasString(string str, object data)
		{
			return data is MidasEntityContainer container
				? string.Format(str, container.GetTimeLeft())
				: str;
		}
	}
}
