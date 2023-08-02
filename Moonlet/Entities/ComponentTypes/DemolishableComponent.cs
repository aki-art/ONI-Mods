using FUtility;
using UnityEngine;

namespace Moonlet.Entities.ComponentTypes
{
	public class DemolishableComponent : BaseComponent
	{
		public override void Apply(GameObject prefab)
		{
			Log.Debuglog("Applying demolishable");
			prefab.AddOrGet<Demolishable>();
		}
	}
}
