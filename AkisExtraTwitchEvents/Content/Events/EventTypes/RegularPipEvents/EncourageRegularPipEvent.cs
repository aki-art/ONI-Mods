using ONITwitchLib;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events.RegularPipEvents
{
	public class EncourageRegularPipEvent : ITwitchEvent
	{
		public const string ID = "EncourageRegularPip";

		public bool Condition(object data) => AkisTwitchEvents.HasUpgradeablePip();

		public int GetWeight() => TwitchEvents.Weights.COMMON;

		public string GetID() => ID;

		public void Run(object data)
		{
			var pip = AkisTwitchEvents.regularPipTarget;

			if (pip == null)
			{
				var newPip = AkisTwitchEvents.GetUpgradeablePip();

				var str = STRINGS.AETE_EVENTS.ENCOURAGE_REGULAR_PIP.SOMETHING_WENT_WRONG;

				if (newPip != null)
				{
					str += "\n\n" +
						STRINGS.AETE_EVENTS.ENCOURAGE_REGULAR_PIP.INSTEAD.Replace("{Name}", Util.StripTextFormatting(newPip.GetProperName()));
				}
				else
				{
					str += "\n\n" +
						STRINGS.AETE_EVENTS.ENCOURAGE_REGULAR_PIP.NOONE;
				}

				ToastManager.InstantiateToastWithGoTarget(STRINGS.AETE_EVENTS.ENCOURAGE_REGULAR_PIP.OHNO, str, newPip.gameObject);

				pip = newPip;
			}

			if (pip != null)
			{
				var skill = pip.LearnNextSkill();

				if (skill == null)
					return;

				var name = Util.StripTextFormatting(pip.GetProperName());

				var toast = STRINGS.AETE_EVENTS.ENCOURAGE_REGULAR_PIP.TOAST
					.Replace("{Name}", name);

				var desc = STRINGS.AETE_EVENTS.ENCOURAGE_REGULAR_PIP.DESC
					.Replace("{Name}", name)
					.Replace("{Skill}", Util.StripTextFormatting(skill));

				ToastManager.InstantiateToastWithGoTarget(toast, desc, pip.gameObject);
			}
		}
	}
}
