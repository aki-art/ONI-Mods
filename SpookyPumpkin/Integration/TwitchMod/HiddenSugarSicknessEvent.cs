using Klei.AI;
using ONITwitchLib;
using SpookyPumpkinSO.Content.Sicknesses;

namespace SpookyPumpkinSO.Integration.TwitchMod
{
	public class HiddenSugarSicknessEvent() : HiddenEventBase(ID)
	{
		public const string ID = "SugarSickness";

		public override Danger GetDanger() => Danger.Small;

		public override int GetNiceness() => Intent.EVIL;

		public override void Run()
		{
			foreach (var minion in Components.LiveMinionIdentities.Items)
			{
				var sicknesses = minion.GetSicknesses();
				if (sicknesses != null)
				{
					var exposure_info = new SicknessExposureInfo(SugarSickness.ID, STRINGS.DUPLICANTS.DISEASES.SP_SICKNESS_SUGARSICKNESS.SOURCE);
					sicknesses.Infect(exposure_info);
				}
			}

			ToastManager.InstantiateToast(
				STRINGS.UI.SPOOKYPUMPKIN.TWITCHEVENTS.TRICKORTREAT.TRICK,
				STRINGS.UI.SPOOKYPUMPKIN.TWITCHEVENTS.SUGARSICKNESS.TOAST_BODY);
		}
	}
}
