using ONITwitchLib;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events.RegularPipEvents
{
	public class EncourageRegularPipEvent() : TwitchEventBase(ID)
	{
		public const string ID = "EncourageRegularPip";

		public override bool Condition() => AkisTwitchEvents.HasUpgradeablePip();

		public override int GetWeight() => Consts.EventWeight.Common;

		public override Danger GetDanger() => Danger.Small;

		public override void Run()
		{
			var pip = AkisTwitchEvents.regularPipTarget;

			if (pip == null)
			{
				var newPip = AkisTwitchEvents.GetUpgradeablePip();

				var str = STRINGS.AETE_EVENTS.ENCOURAGEREGULARPIP.SOMETHING_WENT_WRONG;

				if (newPip != null)
				{
					str += "\n\n" +
						STRINGS.AETE_EVENTS.ENCOURAGEREGULARPIP.INSTEAD.Replace("{Name}", Util.StripTextFormatting(newPip.GetProperName()));
				}
				else
				{
					str += "\n\n" +
						STRINGS.AETE_EVENTS.ENCOURAGEREGULARPIP.NOONE;
				}

				ToastManager.InstantiateToastWithGoTarget(STRINGS.AETE_EVENTS.ENCOURAGEREGULARPIP.OHNO, str, newPip.gameObject);

				pip = newPip;
			}

			if (pip != null)
			{
				var skill = pip.LearnNextSkill();

				if (skill == null)
					return;

				var name = Util.StripTextFormatting(pip.GetProperName());

				var toast = STRINGS.AETE_EVENTS.ENCOURAGEREGULARPIP.TOAST
					.Replace("{Name}", name);

				var desc = STRINGS.AETE_EVENTS.ENCOURAGEREGULARPIP.DESC
					.Replace("{Name}", name)
					.Replace("{Skill}", Util.StripTextFormatting(skill));

				ToastManager.InstantiateToastWithGoTarget(toast, desc, pip.gameObject);
			}
		}
	}
}
