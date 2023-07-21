using ProcGen;
using UnityEngine;

namespace Moonlet.Entities.ComponentTypes
{
	public class RummagableComponent : BaseComponent
	{
		public MinMax DataBanksCount { get; set; }

		public string[][] Contents { get; set; }

		public override void Apply(GameObject prefab)
		{
			var setLocker = prefab.AddOrGet<SetLocker>();
			setLocker.numDataBanks = new[] { (int)DataBanksCount.min, (int)DataBanksCount.max };
			setLocker.possible_contents_ids = Contents;

			prefab.GetComponent<KPrefabID>().prefabInitFn += go =>
			{
				if (go.TryGetComponent(out SetLocker locker))
					locker.ChooseContents();
			};
		}
	}
}
