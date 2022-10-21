using Database;
using SpookyPumpkinSO.Content.Cmps;
using STRINGS;

namespace SpookyPumpkinSO.Content
{
    public class SPStatusItems
    {
        public static StatusItem ghastlyLitBonus;

        public static void Register(DuplicantStatusItems instance)
        {
            ghastlyLitBonus = new StatusItem(
                "SP_GhastlyLitBonus",
                "DUPLICANTS",
                "",
                StatusItem.IconType.Info,
                NotificationType.Good,
                false,
                OverlayModes.None.ID,
                true,
                2);

            ghastlyLitBonus.resolveTooltipCallback = (str, data) =>
            {
                var arg = GameUtil.AddPositiveSign(GameUtil.GetFormattedPercent(Mod.Config.GhastlyWorkBonus * 100f), true);
                return string.Format(str, arg);
            };

            instance.Add(ghastlyLitBonus);
        }
    }
}
