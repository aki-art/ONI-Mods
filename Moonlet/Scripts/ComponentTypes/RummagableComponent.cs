using Moonlet.Templates.SubTemplates;
using UnityEngine;

namespace Moonlet.Scripts.ComponentTypes
{
	internal class RummagableComponent : BaseComponent
	{
		public RummagableData Data { get; private set; }

		public class RummagableData
		{
			public MinMaxC DataBanksCount { get; set; }

			public string[][] Contents { get; set; }
		}

		public override void Apply(GameObject prefab)
		{
			var setLocker = prefab.AddOrGet<SetLocker>();

			var workable = prefab.AddOrGet<Workable>();
			workable.synchronizeAnims = false;
			workable.resetProgressOnStop = true;

			prefab.GetComponent<KPrefabID>().prefabInitFn += go =>
			{
				if (go.TryGetComponent(out SetLocker locker))
				{
					locker.numDataBanks = [(int)Data.DataBanksCount.Min, (int)Data.DataBanksCount.Max];
					locker.possible_contents_ids = Data.Contents;
					locker.ChooseContents();
				}
			};
		}
	}
}
