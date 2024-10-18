using System;
using System.Collections.Generic;

namespace Moonlet.Scripts
{
	public class Moonlet_LightShapePreview : KMonoBehaviour
	{
		public List<LightSource> LightSources;

		[Serializable]
		public struct LightSource
		{
			public float radius;
			public int lux;
			public int width;
			public DiscreteShadowCaster.Direction direction;
			public LightShape shape;
			public CellOffset offset;
		}

		public int previousCell = -1;

		public void AddSource(float radius, int lux, int width, LightShape shape, DiscreteShadowCaster.Direction direction, CellOffset offset)
		{
			LightSources ??= [];
			LightSources.Add(new LightSource
			{
				radius = radius,
				lux = lux,
				width = width,
				direction = direction,
				offset = offset,
				shape = shape
			});
		}

		public void Update()
		{
			int cell = Grid.PosToCell(transform.GetPosition());
			if (cell != previousCell)
			{
				previousCell = cell;
				LightGridManager.DestroyPreview();
				LightGridManager.previewLightCells.Clear();

				if (LightSources == null)
					return;

				foreach (var source in LightSources)
				{
					CreatePreview(Grid.OffsetCell(cell, source.offset), source.radius, source.shape, source.lux, source.width, source.direction);
				}
			}
		}

		public static void CreatePreview(int origin_cell, float radius, LightShape shape, int lux, int width, DiscreteShadowCaster.Direction direction)
		{
			var pooledList = ListPool<int, LightGridManager.LightGridEmitter>.Allocate();
			pooledList.Add(origin_cell);
			DiscreteShadowCaster.GetVisibleCells(origin_cell, pooledList, (int)radius, width, direction, shape);

			foreach (int item in pooledList)
			{
				if (Grid.IsValidCell(item))
				{
					var lightLevel = lux / LightGridManager.ComputeFalloff(0.5f, item, origin_cell, shape, direction);
					LightGridManager.previewLightCells.Add(new Tuple<int, int>(item, lightLevel));
					LightGridManager.previewLux[item] = lightLevel;
				}
			}

			pooledList.Recycle();
		}

		public override void OnCleanUp()
		{
			LightGridManager.DestroyPreview();
		}
	}
}
