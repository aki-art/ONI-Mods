using ONITwitchLib;
using Twitchery.Content.Defs;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class TreeSpawner : KMonoBehaviour
	{
		public Vector3 startPosition;

		[SerializeField] public float minTimeDelay;
		[SerializeField] public float minDistance;
		[SerializeField] public int minAmount;
		[SerializeField] public int maxAmount;

		private float elapsedTime;
		private int totalTrees;
		private int treesSpawned;

		public override void OnSpawn()
		{
			base.OnSpawn();

			totalTrees = Random.Range(minAmount, maxAmount);
			SpawnTree(STRINGS.AETE_EVENTS.TREE.DESC);
		}

		void Update()
		{
			if (treesSpawned >= totalTrees)
			{
				Util.KDestroyGameObject(gameObject);
				return;
			}

			elapsedTime += Time.deltaTime;

			if (elapsedTime > minTimeDelay)
			{
				var currentPos = ONITwitchLib.Utils.PosUtil.ClampedMouseWorldPos();
				var dist = Vector3.Distance(startPosition, currentPos);

				if (dist > minDistance)
				{
					SpawnTree(STRINGS.AETE_EVENTS.TREE.DESC2);
					treesSpawned++;
					elapsedTime = 0;
				}
			}
		}

		private static void SpawnTree(string msg)
		{
			var cell = GetStartCell();
			var branch = FUtility.Utils.Spawn(BranchWalkerConfig.ID, Grid.CellToPos(cell));
			var branchWalker = branch.GetComponent<BranchWalker>();

			branchWalker.Generate();
			ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.TREE.TOAST, msg);
		}

		private static int GetStartCell()
		{
			var cell = Grid.PosToCell(PlayerController.GetCursorPos(KInputManager.GetMousePos()));

			if (Grid.IsValidCell(cell) && Grid.Element[cell].hardness <= 2)
				return cell;

			return ONITwitchLib.Utils.PosUtil.RandomCellNearMouse();
		}
	}
}
