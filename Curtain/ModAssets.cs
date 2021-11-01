using Database;
using STRINGS;

namespace Curtain
{
    public class ModAssets
    {
        public static StatusItem CurtainStatus { get; set; }
        public static ChoreType ToggleCurtainChoreType { get; set; }
        public static Tag plasticTag = TagManager.Create("AC_FlexibleMaterial", "Flexible Material");

        internal static void MakeChoreType()
        {
            ListPool<Tag, ChoreTypes>.PooledList pooledList = ListPool<Tag, ChoreTypes>.Allocate();

            ToggleCurtainChoreType = 
                new ChoreType("ToggleCurtain", Db.Get().ChoreTypes, new string[0], "", 
                DUPLICANTS.CHORES.TOGGLE.NAME, DUPLICANTS.CHORES.TOGGLE.STATUS, 
                DUPLICANTS.CHORES.TOGGLE.TOOLTIP, pooledList, 10000, 5000);

        }
        internal static void MakeStatusItem()
        {
            var item = new StatusItem(
                           id: "ChangeCurtainControlState",
                           prefix: "BUILDING",
                           icon: "status_item_pending_switch_toggle",
                           icon_type: StatusItem.IconType.Custom,
                           notification_type: NotificationType.Neutral,
                           allow_multiples: false,
                           render_overlay: OverlayModes.None.ID)
            {
                resolveStringCallback = delegate (string str, object data)
                {
                    var curtain = (Curtain)data;
                    return str.Replace("{CurrentState}", curtain.RequestedState.ToString());
                }
            };

            CurtainStatus = item;
        }
    }
}