using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes.PandoraSubEvents
{
	public class PandoraEntitySpamEvent(float weight, string prefabId, int minCount, int maxCount) : PandoraEventBase(weight)
	{
		private readonly int minCount = minCount, maxCount = maxCount;

		public override Danger GetDanger() => Danger.Deadly;

		public override void Run(PandorasBox box)
		{
			base.Run(box);

			var thingsToSpawn = Random.Range(minCount, maxCount);
			for (var i = 0; i < thingsToSpawn; i++)
			{
				var crabby = FUtility.Utils.Spawn(prefabId, box.gameObject, Grid.SceneLayer.Move);
				FUtility.Utils.YeetRandomly(crabby, false, 4, 10, false);
			}

			End();
		}
	}
}
