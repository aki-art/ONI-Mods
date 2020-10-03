namespace ModularStorage
{
    class ModAssets
    {
        public static string ModPath;
        public static StatusItem NotConnectedModuleStatus;
        public static StatusItem ConnectionStatus;
        internal static void MakeStatusItems()
        {
            var item = new StatusItem(
                           id: "NotConnectedModule",
                           prefix: "BUILDING",
                           icon: "status_item_exclamation",
                           icon_type: StatusItem.IconType.Custom,
                           notification_type: NotificationType.Bad,
                           allow_multiples: false,
                           render_overlay: OverlayModes.None.ID);

            NotConnectedModuleStatus = item;


            var item2 = new StatusItem(
                           id: "ConnectionStatus",
                           prefix: "BUILDING",
                           icon: "status_item_exclamation",
                           icon_type: StatusItem.IconType.Custom,
                           notification_type: NotificationType.Neutral,
                           allow_multiples: false,
                           render_overlay: OverlayModes.None.ID);

            ConnectionStatus = item2;
        }
    }
}
