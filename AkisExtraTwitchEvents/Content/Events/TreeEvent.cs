using ONITwitchLib;
using Twitchery.Content.Defs;
using Twitchery.Content.Scripts;

namespace Twitchery.Content.Events
{
	internal class TreeEvent : ITwitchEvent
	{
		public const string ID = "Tree";

		public bool Condition(object data) => true;

		public string GetID() => ID;

		public void Run(object data)
		{
			var cell = ONITwitchLib.Utils.PosUtil.RandomCellNearMouse();
			var branch = FUtility.Utils.Spawn(BranchWalkerConfig.ID, Grid.CellToPos(cell));
			var branchWalker = branch.GetComponent<BranchWalker>();

			branchWalker.Generate();

			ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.TREE.TOAST, STRINGS.AETE_EVENTS.TREE.DESC);
		}
	}
}
