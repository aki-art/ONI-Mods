namespace Curtain
{
    public class ModAssets
    {
        public static StatusItem ChangeCurtainControlState { get; set; }

        internal static void MakeStatusItem()
        {
            var item = new StatusItem(
                           id: "ChangeCurtainControlState",
                           prefix: "BUILDING",
                           icon: "status_item_pending_switch_toggle",
                           icon_type: StatusItem.IconType.Custom,
                           notification_type: NotificationType.Neutral,
                           allow_multiples: false,
                           render_overlay: OverlayModes.None.ID);

            item.resolveStringCallback = delegate (string str, object data)
            {
                var curtain = (Curtain)data;
                return str.Replace("{CurrentState}", curtain.RequestedState.ToString());
            };

            ChangeCurtainControlState = item;
        }
    }
}
