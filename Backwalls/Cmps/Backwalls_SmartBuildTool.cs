using KSerialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Backwalls.Cmps
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Backwalls_SmartBuildTool : KMonoBehaviour//, IInputHandler
	{
		[Serialize] public bool smartCursorEnabled;

		private const int LARGE_CAVITY_R = 7;
		private const int MAX_CELLS = (LARGE_CAVITY_R * 2 + 1) * (LARGE_CAVITY_R * 2 + 1);
		private bool hasHijackedPosition;
		private bool isActive;

		private BuildingDef activeDef;

		public static Backwalls_SmartBuildTool Instance;

		public static int OverrideTargetCell { private set; get; }
		public string handlerName => nameof(Backwalls_SmartBuildTool);
		public KInputHandler inputHandler { get; set; }

		private List<int> connectedCells;
		private readonly HashSet<int> alreadyUsedCells = [];

		private int lastQueryCell = -1;
		private CellData cellData;

		bool searchForNext;
		bool isMouseDown;
		int currentCell;

		//float elapsedHoldingTime;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Instance = this;

			//Global.GetInputManager().GetDefaultController().GetKeyDown(KKeyCode.Mouse0)
		}


		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Instance = null;
		}

		public void CollectConnectedCells(int cell)
		{
			connectedCells ??= [];

			var cavity = Game.Instance.roomProber.GetCavityForCell(cell);
			var worldIdx = Grid.IsValidCell(cell) ? Grid.WorldIdx[cell] : -1;

			if (worldIdx == -1)
			{
				Log.Warning("Trying to build outside of a valid world.");
				return;
			}

			cellData = new CellData()
			{
				cavity = cavity,
				cellOrigin = cell,
				worldIdx = worldIdx
			};

			var isStartValid = !Grid.Solid[cell]
				&& !Grid.Foundation[cell]
				&& (cellData.cavity == null || Game.Instance.roomProber.GetCavityForCell(cell) == cellData.cavity)
				&& Grid.IsValidCellInWorld(cell, cellData.worldIdx);

			if (!isStartValid)
			{
				connectedCells.Clear();
				return;
			}

			if (lastQueryCell != cell || connectedCells.Count == 0)
			{
				connectedCells = [];

				var cells = GameUtil.FloodCollectCells(cell, cell => IsValidCell(cell, true, cellData), MAX_CELLS, clearOversizedResults: false);

				connectedCells = cells
					.Where(cell => !CellHasWall(cell))
					.OrderBy(c => Grid.GetCellDistance(cell, c))
					.ToList();

				lastQueryCell = cell;
			}
		}

		private bool IsValidCell(int cell, bool canBeOccupied, CellData data)
		{
			if (data == null)
			{
				Log.Warning($"{nameof(Backwalls_SmartBuildTool)} {nameof(IsValidCell)} null data");
				return false;
			}

			if (data.cellOrigin == cell)
			{
				return !Grid.Solid[cell]
					&& !Grid.Foundation[cell]
					&& !Grid.ObjectLayers[(int)ObjectLayer.Backwall].ContainsKey(cell)
					&& Grid.IsValidCellInWorld(cell, data.worldIdx);
			}

			// for giant open cavities and open space use a bounding box
			if (data.cavity != null && data.cavity.numCells > MAX_CELLS)
			{
				Grid.CellToXY(cell, out int x, out int y);
				Grid.CellToXY(cellData.cellOrigin, out int xo, out int yo);
				var xDist = Mathf.Abs(x - xo);
				var yDist = Mathf.Abs(y - yo);

				if (xDist > LARGE_CAVITY_R || yDist > LARGE_CAVITY_R)
					return false;
			}

			if (!canBeOccupied)
			{
				if (connectedCells.Contains(cell) || CellHasWall(cell))
					return false;
			}

			return !Grid.Solid[cell]
				//&& BuildTool.Instance.def.IsValidPlaceLocation(BuildTool.Instance.def.BuildingPreview, cell, Orientation.Neutral, out var _)
				&& !Grid.Foundation[cell]
				&& (data.cavity == null || Game.Instance.roomProber.GetCavityForCell(cell) == data.cavity)
				&& Grid.IsValidCellInWorld(cell, data.worldIdx);
		}

		private int PopNext()
		{
			if (connectedCells.Count == 0)
				return -1;

			var result = connectedCells[0];
			connectedCells.RemoveAt(0);

			OverrideTargetCell = result;

			return result;
		}

		private bool CellHasWall(int cell) => Grid.ObjectLayers[(int)ObjectLayer.Backwall].ContainsKey(cell);

		void Update()
		{
			/*			if (!isActive && isMouseDown)
						{
							elapsedHoldingTime += Time.deltaTime;
							if (elapsedHoldingTime > 0.5f)
							{
								smartCursorEnabled = true;
							}
						}*/
			if (isActive && isMouseDown && OverrideTargetCell > -1 && !alreadyUsedCells.Contains(OverrideTargetCell))
			{
				BuildTool.Instance.TryBuild(OverrideTargetCell);
				searchForNext = true;
				alreadyUsedCells.Add(OverrideTargetCell);
				//BuildTool.Instance.UpdateVis(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
			}
		}

		public void OnUpdateVis(ref Vector3 pos)
		{
			if (!smartCursorEnabled)
				return;

			if (OverrideTargetCell == -1 || !Grid.IsValidCellInWorld(OverrideTargetCell, ClusterManager.Instance.activeWorldId))
				return;

			pos = Grid.CellToPos(OverrideTargetCell);
		}

		public IEnumerator TargetSmartCell()
		{
			while (isActive)
			{
				if (searchForNext)
				{
					OverrideTargetCell = PopNext();
					BuildTool.Instance.UpdateVis(PlayerController.GetCursorPos(KInputManager.GetMousePos()));
					searchForNext = false;
				}

				yield return new WaitForSecondsRealtime(0.02f);
			}

			yield return null;
		}

		public void UpdateVisColor(BuildingDef def, GameObject visualizer)
		{
			if (visualizer == null || OverrideTargetCell == -1)
				return;

			if (IsEnabledBuilding(def) && smartCursorEnabled)
				if (visualizer.TryGetComponent(out KBatchedAnimController kbac))
					kbac.TintColour = Color.yellow;
		}

		public void Toggle()
		{
			smartCursorEnabled = !smartCursorEnabled;
		}

		public void OnDragTool(BuildingDef def, ref int cell, ref int lastDragCell)
		{
			if (IsEnabledBuilding(def) && smartCursorEnabled)
			{
				isMouseDown = true;
				searchForNext = true;
				isActive = true;

				CollectConnectedCells(cell);
				StartCoroutine(TargetSmartCell());
			}
		}

		public void OnLeftClickUp(BuildingDef def, ref int lastDragCell)
		{
			isMouseDown = false;
			isActive = false;
			lastQueryCell = -1;
			OverrideTargetCell = -1;
			//elapsedHoldingTime = 0;
			alreadyUsedCells.Clear();
			StopCoroutine(TargetSmartCell());
		}

		private bool IsEnabledBuilding(BuildingDef def) => def != null && def.ObjectLayer == ObjectLayer.Backwall;

		internal bool IsToolActive() => smartCursorEnabled;

		public class CellData
		{
			public int cellOrigin;
			public int worldIdx;
			public CavityInfo cavity;
		}
	}
}
