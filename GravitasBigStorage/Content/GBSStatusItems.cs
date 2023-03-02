namespace GravitasBigStorage.Content
{
    public class GBSStatusItems
    {
        public static StatusItem beingStudied;

        public static void Register()
        {
            beingStudied = new(
                "GravitasBigStorage_BeingStudied",
                "DUPLICANTS",
                string.Empty,
                StatusItem.IconType.Info,
                NotificationType.Neutral,
                false,
                OverlayModes.None.ID);
        }
    }
}
