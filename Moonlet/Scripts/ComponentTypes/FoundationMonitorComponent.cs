using Moonlet.Templates.SubTemplates;
using UnityEngine;

namespace Moonlet.Scripts.ComponentTypes
{
	public class FoundationMonitorComponent : BaseComponent
	{
		public FoundationMonitorData Data { get; private set; }

		public class FoundationMonitorData
		{
			public Vector2IC[] Offsets { get; set; }
			public bool DestroyOnFoundationLost { get; set; }
		}

		public override void Apply(GameObject prefab)
		{
			var cellOffsets = new CellOffset[Data.Offsets == null ? 1 : Data.Offsets.Length];

			if (Data.Offsets == null)
				cellOffsets[0] = CellOffset.down;
			else
			{
				for (var i = 0; i < Data.Offsets.Length; i++)
					cellOffsets[i] = Data.Offsets[i].ToCellOffset();
			}

			prefab.AddOrGet<FoundationMonitor>().monitorCells = cellOffsets;

			if (Data.DestroyOnFoundationLost)
				prefab.AddOrGet<MoonletComponentHolder>().destroyOnFoundationLost = true;
		}
	}
}
