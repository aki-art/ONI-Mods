using Moonlet.Content.Scripts;
using TUNING;
using UnityEngine;

namespace Moonlet.Entities.ComponentTypes
{
	internal class StorageComponent : BaseComponent
	{
		public float Capacity { get; set; }

		public string Filters { get; set; }

		public override void Apply(GameObject prefab)
		{
			var storage = prefab.AddComponent<Storage>();
			storage.capacityKg = Capacity;
			storage.storageFilters = MiscUtil.GetStorageFilterFromName(Filters, out _) ?? STORAGEFILTERS.NOT_EDIBLE_SOLIDS;
			
		}
	}
}
