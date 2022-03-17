using DuctTape.Components;
using STRINGS;

namespace DuctTape
{
    public class ModAssets
    {
        public static class Tags
        {
            public static readonly Tag ductTapeable = TagManager.Create("DuctTapeable");
            public static readonly Tag ductTape = TagManager.Create("DuctTape");

            // manually assigned to buildings we don't want duct tape to be attached. 
            // can be used by other mods to fix incorrect interactions
            public static readonly Tag preventDuctTape = TagManager.Create("PreventDuctTape");
        }

        public static class StatusItems
        {
            public static StatusItem notConnected;

            public static void Register()
            {
                notConnected = new StatusItem(
                    "NotLinkedToHeadStatusItem",
                    BUILDING.STATUSITEMS.NOTLINKEDTOHEAD.NAME,
                    BUILDING.STATUSITEMS.NOTLINKEDTOHEAD.TOOLTIP,
                    "status_item_not_linked", StatusItem.IconType.Custom,
                    NotificationType.BadMinor,
                    false,
                    OverlayModes.None.ID);

                notConnected.resolveTooltipCallback = (tooltip, obj) =>
                {
                    if (obj is DuctTapeConnection.StatesInstance cbi)
                    {
                        return tooltip
                            .Replace("{headBuilding}", Strings.Get("STRINGS.BUILDINGS.PREFABS." + cbi.def.headBuildingTag.Name.ToUpper() + ".NAME"));
                    }

                    else return tooltip;
                };
            }
        }
    }
}
