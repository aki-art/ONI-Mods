using UnityEngine;

namespace Moonlet.Entities.ComponentTypes
{
	public class DemolishableComponent : BaseComponent
	{
		public override void Apply(GameObject prefab)
		{
			prefab.AddComponent<Demolishable>();
		}
	}
}
