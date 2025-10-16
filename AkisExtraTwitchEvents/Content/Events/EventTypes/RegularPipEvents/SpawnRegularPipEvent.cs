#if SUPERPIP
using ONITwitchLib;
using ONITwitchLib.Utils;
using Twitchery.Content.Defs;

namespace Twitchery.Content.Events.RegularPipEvents
{
	public class SpawnRegularPipEvent() : TwitchEventBase(ID)
	{
		public const string ID = "SpawnRegularPip";

		public string GetID() => ID;

		public override Danger GetDanger()
		{
			throw new System.NotImplementedException();
		}

		public override int GetWeight() => Mod.regularPips.Count > 0 ? TwitchEvents.Weights.VERY_RARE : TwitchEvents.Weights.COMMON;

		public override void Run()
		{
			var telepad = GameUtil.GetActiveTelepad();

			var pos = telepad == null
				? Grid.CellToPos(PosUtil.RandomCellNearMouse())
				: telepad.transform.position;

			var pip = FUtility.Utils.Spawn(TentaclePortalConfig.ID, pos);
			;
			ToastManager.InstantiateToastWithGoTarget(
				STRINGS.AETE_EVENTS.REGULARPIP.TOAST,
				STRINGS.AETE_EVENTS.REGULARPIP.DESC,
				pip);
		}
	}
}

#endif