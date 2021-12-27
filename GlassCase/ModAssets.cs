using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassCase
{
    internal class ModAssets
    {
        public static StatusItem connectedStatus;
        public static StatusItem disconnectedStatus;

        internal static void CreateStatusItem()
        {
            connectedStatus = new StatusItem(
                           Mod.ID + "UnconfiguredStatus",
                           "BUILDING",
                           "status_item_pending_switch_toggle",
                           StatusItem.IconType.Custom,
                           NotificationType.Good,
                           false,
                           OverlayModes.None.ID);

            disconnectedStatus = new StatusItem(
                           Mod.ID + "DisconnectedStatus",
                           "BUILDING",
                           "status_item_pending_switch_toggle",
                           StatusItem.IconType.Custom,
                           NotificationType.Neutral,
                           false,
                           OverlayModes.None.ID);
        }
    }
}
