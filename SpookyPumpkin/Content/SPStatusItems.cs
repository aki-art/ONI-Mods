using Database;
using FUtility;

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
				var workspeed = GameUtil.AddPositiveSign(GameUtil.GetFormattedPercent(Mod.Config.GhastlyWorkBonus), true);
				var stress = GameUtil.AddPositiveSign(GameUtil.GetFormattedPercent(-Mod.Config.GhastlyStressBonus / CONSTS.CYCLE_LENGTH), Mod.Config.GhastlyStressBonus >= 0);
				return string.Format(str, workspeed, stress);
			};

			instance.Add(ghastlyLitBonus);
		}
	}
}
