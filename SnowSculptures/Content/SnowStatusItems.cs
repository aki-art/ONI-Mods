using SnowSculptures.Content.Buildings;

namespace SnowSculptures.Content
{
    public class SnowStatusItems
    {
        public static StatusItem sealedStatus;

        public static void CreateStatusItems()
        {
            sealedStatus = new StatusItem(
                "SnowSculptures_SealedStatusItem",
                "BUILDINGS",
                "",
                StatusItem.IconType.Info,
                NotificationType.Neutral,
                false,
                OverlayModes.None.ID);

            sealedStatus.SetResolveStringCallback((str, obj) =>
            {
                if(obj is GlassCaseSealable sealable)
                {
                    return (sealable.glassCase != null && sealable.glassCase.broken) ? (string)STRINGS.BUILDINGS.STATUSITEMS.SNOWSCULPTURES_SOMEHOWSEALEDSTATUSITEM.NAME : str;
                }

                return str;
            });
        }
    }
}
