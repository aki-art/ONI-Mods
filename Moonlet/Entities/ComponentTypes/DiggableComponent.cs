using FUtility;
using UnityEngine;

namespace Moonlet.Entities.ComponentTypes
{
	public class DiggableComponent : BaseComponent
	{
		public string ChoreType { get; set; }

		public override void Apply(GameObject prefab)
		{
			var diggable = prefab.AddOrGet<Diggable>();

			if (!ChoreType.IsNullOrWhiteSpace())
			{
				var choreType = Db.Get().ChoreTypes.TryGet(ChoreType);
				if (choreType == null)
				{
					if (!Optional)
						Log.Warning("There is no choretype with ID " + ChoreType);

					return;
				}

				diggable.choreTypeIdHash = ChoreType;
			}
		}
	}
}
