using ONITwitchLib;
using ONITwitchLib.Utils;
using Twitchery.Content.Defs;

namespace Twitchery.Content.Events.RegularPipEvents
{
	public class SpawnRegularPipEvent : ITwitchEvent
	{
		public const string ID = "SpawnRegularPip";

		public int GetWeight() => Mod.regularPips.Count > 0 ? TwitchEvents.Weights.VERY_RARE : TwitchEvents.Weights.COMMON;

		public bool Condition(object data) => true;

		public string GetID() => ID;

		public void Run(object data)
		{
			var telepad = GameUtil.GetActiveTelepad();

			var pos = telepad == null
				? Grid.CellToPos(PosUtil.RandomCellNearMouse())
				: telepad.transform.position;

			var pip = FUtility.Utils.Spawn(TentaclePortalConfig.ID, pos);
			;
			ToastManager.InstantiateToastWithGoTarget(
				STRINGS.AETE_EVENTS.REGULAR_PIP.TOAST,
				STRINGS.AETE_EVENTS.REGULAR_PIP.DESC,
				pip);
		}
	}
}

