using UnityEngine;

namespace Moonlet.Scripts.Commands
{
	public class DropOwnMaterialsCommand : BaseCommand
	{
		public override void Run(object data)
		{
			if (data is not GameObject go)
				return;

			if (go.TryGetComponent(out PrimaryElement primaryElement))
			{
				var item = FUtility.Utils.Spawn(primaryElement.Element.tag, go.transform.position);

				if (item == null)
					return;

				var spawnedPe = item.GetComponent<PrimaryElement>();

				spawnedPe.Mass = primaryElement.Mass;
				spawnedPe.Temperature = primaryElement.Temperature;

				if (primaryElement.DiseaseIdx != byte.MaxValue)
					spawnedPe.AddDisease(primaryElement.DiseaseIdx, primaryElement.DiseaseCount, "spawn self materials");
			}
		}
	}
}
