using Database;
using Twitchery.Content.Defs;
using Twitchery.Content.Scripts;

namespace Twitchery.Content
{
    public class TStatusItems
    {
        public static StatusItem CalorieStatus;

        public static void Register(MiscStatusItems parent)
        {
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
        }
    }
}
