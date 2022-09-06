using ZipLine.Content.Entities;

namespace ZipLine
{
    internal class ModDb
    {
        public class StatusItems
        {
            public static StatusItem ziplineConnected;
            public static StatusItem distance;

            public static void Register()
            {
                ziplineConnected = new StatusItem(
                    "Zipline_ZiplineConnected",
                    "BUILDINGS",
                    string.Empty,
                    StatusItem.IconType.Info,
                    NotificationType.Neutral,
                    false,
                    OverlayModes.None.ID,
                    false);

                distance = new StatusItem(
                    "Zipline_Distance",
                    "BUILDINGS",
                    string.Empty,
                    StatusItem.IconType.Info,
                    NotificationType.Neutral,
                    false,
                    OverlayModes.None.ID,
                    false);

                distance.SetResolveStringCallback((str, data) => data is Rope rope ? string.Format(str, GameUtil.GetFormattedDistance(rope.distance)) : str);
            }
        }
    }
}
