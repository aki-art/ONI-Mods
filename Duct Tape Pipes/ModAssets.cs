using DuctTapePipes.Buildings;

namespace DuctTapePipes
{
    public class ModAssets
    {
        public static StatusItem unconfiguredStatus;
        public static StatusItem connectedStatus;
        public static StatusItem disconnectedStatus;

        internal static void CreateStatusItem()
        {
            unconfiguredStatus = new StatusItem(
                           Mod.ID + "UnconfiguredStatus",
                           "BUILDING",
                           "status_item_pending_switch_toggle",
                           StatusItem.IconType.Custom,
                           NotificationType.Neutral,
                           false,
                           OverlayModes.None.ID);

            disconnectedStatus = new StatusItem(
                           Mod.ID + "DisconnectedStatus",
                           "BUILDING",
                           "status_item_pending_switch_toggle",
                           StatusItem.IconType.Custom,
                           NotificationType.BadMinor,
                           false,
                           OverlayModes.None.ID);

            connectedStatus = new StatusItem(
                           Mod.ID + "ConnectedStatus",
                           "BUILDING",
                           "status_item_pending_switch_toggle",
                           StatusItem.IconType.Custom,
                           NotificationType.Good,
                           false,
                           OverlayModes.None.ID);
            /*
            {
                resolveStringCallback = (str, data) =>
                {
                    var leech = (Leech)data;
                    return str.Replace("{ConnectedBuildingName}", leech.Storage.GetProperName());
                }
            };*/

        }

    }
}
