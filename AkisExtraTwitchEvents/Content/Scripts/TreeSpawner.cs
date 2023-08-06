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
		float elapsedTime;
		public bool spawnedSecondTree;

		public override void OnSpawn()
		{
			base.OnSpawn();

			var startPosition = ONITwitchLib.Utils.PosUtil.ClampedMouseWorldPos();
			SpawnTree(STRINGS.AETE_EVENTS.TREE.DESC);
		}

		void Update()
		{
			if (spawnedSecondTree)
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
					spawnedSecondTree = true;
				}
			}
		}

		private static void SpawnTree(string msg)
		{
			var cell = ONITwitchLib.Utils.PosUtil.RandomCellNearMouse();
			var branch = FUtility.Utils.Spawn(BranchWalkerConfig.ID, Grid.CellToPos(cell));
			var branchWalker = branch.GetComponent<BranchWalker>();

			branchWalker.Generate();
			ToastManager.InstantiateToast(STRINGS.AETE_EVENTS.TREE.TOAST, msg);
		}
	}
}
