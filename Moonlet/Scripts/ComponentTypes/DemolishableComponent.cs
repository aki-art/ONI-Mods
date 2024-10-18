using UnityEngine;

namespace Moonlet.Scripts.ComponentTypes
{
	public class DemolishableComponent : BaseComponent
	{
		public override void Apply(GameObject prefab)
		{
			prefab.AddOrGet<Demolishable>();
		}
	}
}
