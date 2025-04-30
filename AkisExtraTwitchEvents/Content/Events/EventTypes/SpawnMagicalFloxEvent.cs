using ONITwitchLib;
using ONITwitchLib.Utils;
using Twitchery.Content.Defs.Critters;

namespace Twitchery.Content.Events.EventTypes
{
	public class SpawnMagicalFloxEvent() : TwitchEventBase(ID)
	{
		public const string ID = "SpawnMagicalFlox";

		public override Danger GetDanger() => Danger.Small;

		public override int GetWeight() => Consts.EventWeight.Uncommon;

		public override void Run()
		{
			var cell = GridUtil.NearestEmptyCell(PosUtil.RandomCellNearMouse());
			var pos = Grid.CellToPos(cell);

			var flox = FUtility.Utils.Spawn(MagicalFloxConfig.ID, pos);
			ToastManager.InstantiateToastWithGoTarget(STRINGS.AETE_EVENTS.SPAWNMAGICALFLOX.TOAST, STRINGS.AETE_EVENTS.SPAWNMAGICALFLOX.DESC, flox);
		}
	}
}
