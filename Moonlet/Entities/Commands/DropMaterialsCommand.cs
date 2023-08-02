using FUtility;
using UnityEngine;

namespace Moonlet.Entities.Commands
{
	public class DropMaterialsCommand : BaseCommand
	{
		public override void Run(GameObject go)
		{
			Log.Debuglog("run drop materials command");
			if (go.TryGetComponent(out PrimaryElement primaryElement))
			{
				var item = Utils.Spawn(primaryElement.Element.tag, go.transform.position);

				if (item == null)
					return;

				Log.Debuglog("spawned item " + item.PrefabID());

				var spawnedPe = item.GetComponent<PrimaryElement>();

				spawnedPe.Mass = primaryElement.Mass;
				spawnedPe.Temperature = primaryElement.Temperature;

				if (primaryElement.DiseaseIdx != byte.MaxValue)
					spawnedPe.AddDisease(primaryElement.DiseaseIdx, primaryElement.DiseaseCount, "spawn self materials");
			}
		}
	}
}
