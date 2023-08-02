using UnityEngine;

namespace Moonlet.Entities.ComponentTypes
{
	public class FoundationMonitorComponent : BaseComponent
	{
		public Vector2I[] Offsets { get; set; }

		public override void Apply(GameObject prefab)
		{
			var cellOffsets = new CellOffset[Offsets == null ? 1 : Offsets.Length];

			if (Offsets == null)
				cellOffsets[0] = CellOffset.down;
			else
			{
				for (var i = 0; i < Offsets.Length; i++)
					cellOffsets[i] = new CellOffset(Offsets[i]);
			}

			prefab.AddOrGet<FoundationMonitor>().monitorCells = cellOffsets;
		}
	}
}
