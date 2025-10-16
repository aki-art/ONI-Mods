using KSerialization;
using System.Collections.Generic;
using Twitchery.Utils;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class TileEater : KMonoBehaviour, ISim200ms
	{
		[SerializeField] public int brushRadius;
		[SerializeField] public float strength;
		[SerializeField] public float entityDamage;
		[SerializeField] public int buildingDamage;
		[SerializeField] public bool destroyTiles;
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
		[Serialize] private bool _firstFrameSkipped;

		public TileEater()
		{
			preventCrossWorldDamage = true;
			canBreakNeutronium = false;
			destroyTiles = true;
			strength = 1f;
			brushRadius = 1;
		}

		public virtual void UpdateBrushSize()
		{
			_brushOffsets.Clear();

			var brushRadiusVec = new Vector2(brushRadius, brushRadius);

			for (var x = 0; x < brushRadius * 2; ++x)
			{
				for (var y = 0; y < brushRadius * 2; ++y)
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
				var cell = Grid.OffsetCell(currentCell, new CellOffset((int)offset.x, (int)offset.y));

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
			foreach (var cell in _cellsInRadius)
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

		private void ChompEntities(int cell)
		{
			var entries = ListPool<ScenePartitionerEntry, MidasToucher>.Allocate();

			GameScenePartitioner.Instance.GatherEntries(
				Extents.OneCell(cell),
				GameScenePartitioner.Instance.pickupablesLayer,
				entries);

			foreach (var entry in entries)
			{
				if (entry.obj is Pickupable pickupable)
				{
					if (pickupable.HasAnyTags(MidasEntityContainer.ignoreTags))
						continue;

					if (pickupable.TryGetComponent(out Health health))
						health.Damage(entityDamage);
				}
			}
		}

		private void ChompBuildings(int cell)
		{
			var entries = ListPool<ScenePartitionerEntry, MidasToucher>.Allocate();

			GameScenePartitioner.Instance.GatherEntries(
				Extents.OneCell(cell),
				GameScenePartitioner.Instance.completeBuildings,
				entries);

			foreach (var entry in entries)
			{
				if (entry.obj is BuildingComplete building)
				{
					if (building.TryGetComponent(out BuildingHP health))
						health.DoDamage(buildingDamage);
				}
			}
		}

		public void Sim200ms(float dt)
		{
			if (!isActive)
				return;

			if (!_firstFrameSkipped)
				_firstFrameSkipped = true;

			var currentPos = (Vector2)transform.position + offset;

			var cells = ProcGen.Util.GetLine(currentPos, _lastPosition);

			// probably teleported, skip
			if (cells.Count > 32)
			{
				_lastPosition = currentPos;
				return;
			}

			_recentlyVisitedCells.Clear();

			foreach (var cell in cells)
				CollectAffectedCells(Grid.PosToCell(cell));

			foreach (var cell in _recentlyVisitedCells)
			{
				ChompEntities(cell);
				ChompBuildings(cell);

				if (!AGridUtil.Destroyable(cell, true))
					continue;

				if (destroyTiles)
				{
					if (!AGridUtil.DestroyTile(cell))
						AGridUtil.Vacuum(cell);
				}
				else
				{
					WorldDamage.Instance.ApplyDamage(cell, strength, -1);
				}
			}

			_lastPosition = currentPos;
		}
	}
}
