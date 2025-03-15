using System.Collections.Generic;
using Twitchery.Utils;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class TileEater : KMonoBehaviour, ISim200ms
	{
		[SerializeField] public int brushRadius;
		[SerializeField] public int strength;
		[SerializeField] public bool breakTiles;
		[SerializeField] public bool canBreakNeutronium;
		[SerializeField] public bool preventCrossWorldDamage;
		[SerializeField] public Vector2 offset;
		[SerializeField] public bool affectFoundation;

		private Vector2 _lastPosition;
		protected List<Vector2> _brushOffsets = [];
		private int _lastCell;
		private HashSet<int> _cellsInRadius;
		protected HashSet<int> _recentlyVisitedCells = [];
		private int _myWorldIdx;

		public TileEater()
		{
			preventCrossWorldDamage = true;
			canBreakNeutronium = false;
			breakTiles = true;
			strength = 100;
			brushRadius = 1;
		}

		public virtual void UpdateBrushSize()
		{
			_brushOffsets.Clear();

			var brushRadiusVec = new Vector2(brushRadius, brushRadius);

			for (int x = 0; x < brushRadius * 2; ++x)
			{
				for (int y = 0; y < brushRadius * 2; ++y)
				{
					if (Vector2.Distance(new Vector2I(x, y), brushRadiusVec) < brushRadius - 0.8f)
						_brushOffsets.Add(new Vector2(x - brushRadius, y - brushRadius));
				}
			}
		}

		public void CollectAffectedCells(int currentCell)
		{
			if (!isActive)
				return;

			if (_lastCell == currentCell)
				return;

			_cellsInRadius.Clear();

			foreach (var offset in _brushOffsets)
			{
				int cell = Grid.OffsetCell(currentCell, new CellOffset((int)offset.x, (int)offset.y));

				if (Grid.IsValidCell(cell)
					&& Grid.WorldIdx[cell] == _myWorldIdx)
					_cellsInRadius.Add(Grid.OffsetCell(currentCell, new CellOffset((int)offset.x, (int)offset.y)));
			}

			Paint();
			_lastCell = currentCell;
		}

		protected virtual void OnPaintCell(int cell)
		{
			_recentlyVisitedCells.Add(cell);
		}

		private void Paint()
		{
			foreach (int cell in _cellsInRadius)
			{
				if (Grid.IsValidCell(cell)
					&& Grid.WorldIdx[cell] == _myWorldIdx
					&& (!Grid.Foundation[cell] || affectFoundation))

					OnPaintCell(cell); //Grid.GetCellDistance(currentCell, cell)
			}
		}

		public bool isActive;

		public override void OnSpawn()
		{
			base.OnSpawn();
			_recentlyVisitedCells = [];
			_cellsInRadius = [];
			_myWorldIdx = this.GetMyWorldId();
			UpdateBrushSize();
		}


		protected Vector3 ClampPositionToWorld(Vector3 position, WorldContainer world)
		{
			position.x = Mathf.Clamp(position.x, world.minimumBounds.x, world.maximumBounds.x);
			position.y = Mathf.Clamp(position.y, world.minimumBounds.y, world.maximumBounds.y);
			return position;
		}

		public void Sim200ms(float dt)
		{
			if (!isActive)
				return;

			var currentPos = (Vector2)transform.position + offset;

			var cells = ProcGen.Util.GetLine(currentPos, _lastPosition);

			_recentlyVisitedCells.Clear();

			foreach (var cell in cells)
				CollectAffectedCells(Grid.PosToCell(cell));

			foreach (var cell in _recentlyVisitedCells)
			{
				if (AGridUtil.Destroyable(cell, true))
				{
					if (!AGridUtil.DestroyTile(cell))
						AGridUtil.Vacuum(cell);
				}
			}

			_lastPosition = currentPos;
		}
	}
}
