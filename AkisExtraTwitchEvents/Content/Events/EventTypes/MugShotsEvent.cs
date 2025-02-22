using ONITwitchLib;
using ONITwitchLib.Utils;
using System.Collections;
using Twitchery.Content.Defs;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class MugShotsEvent() : TwitchEventBase(ID)
	{
		public const string ID = "MugShots";

		public int cometsToSpawn = 0;
		public float searchRadius = 4;

		public override Danger GetDanger() => Danger.Small;

		//public override bool Condition() => Game.Instance.savedInfo.discoveredSurface;

		public override int GetWeight() => Consts.EventWeight.Common;

		public override void Run()
		{
			cometsToSpawn = Random.Range(8, 14);
			AkisTwitchEvents.Instance.StartCoroutine(SpawnCometsCoroutine());
		}

		private IEnumerator SpawnCometsCoroutine()
		{
			while (cometsToSpawn-- > 0)
			{
				var tries = 16;
				bool success = false;

				while (tries-- > 0 && !success)
				{
					var fromPos = Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos());
					var randomPos = (Vector3)Random.insideUnitCircle * searchRadius;
					randomPos += fromPos;

					var cell = Grid.PosToCell(randomPos);
					//if (Grid.TestLineOfSight((int)randomPos.x, (int)randomPos.y, (int)fromPos.x, (int)fromPos.y, IsCellVisible))
					if (GridUtil.IsCellEmpty(cell))
					{
						var comet = FUtility.Utils.Spawn(MugCometConfig.ID, randomPos).GetComponent<MugComet>();
						comet.aimAtTarget = true;
						comet.target = fromPos;
						if (cometsToSpawn == 1)
							comet.chanceToBreak = 0f;

						comet.RandomizeVelocity();

						success = true;
					}
				}

				yield return SequenceUtil.WaitForSeconds(Random.Range(0.1f, 0.3f));
			}
		}

		private bool IsCellVisible(int cell)
		{
			return !GridUtil.IsCellEmpty(cell);// && Grid.WorldIdx[cell] == ClusterManager.Instance.activeWorldId;
		}
	}
}
