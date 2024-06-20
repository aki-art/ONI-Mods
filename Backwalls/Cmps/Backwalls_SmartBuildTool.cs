using HarmonyLib;
using KSerialization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Backwalls.Cmps
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class Backwalls_SmartBuildTool : KMonoBehaviour, IInputHandler
	{
		[Serialize] public bool smartCursorEnabled;

		private bool hasHijackedPosition;
		private bool isActive;

		private BuildingDef activeDef;

		public static Backwalls_SmartBuildTool Instance;

		public static int OverrideTargetCell { private set; get; }
		public string handlerName => nameof(Backwalls_SmartBuildTool);
		public KInputHandler inputHandler { get; set; }

		private List<int> connectedCells;
		private HashSet<int> alreadyUsedCells = new();

		private int lastQueryCell = -1;
		private CellData cellData;

		bool searchForNext;
		bool isMouseDown;
		int currentCell;

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
			connectedCells ??= new();

			var cavity = Game.Instance.roomProber.GetCavityForCell(cell);
			var worldIdx = Grid.IsValidCell(cell) ? Grid.WorldIdx[cell] : -1;

			if (worldIdx == -1)
			{
				return;
			}

			cellData = new CellData()
			{
				cavity = cavity,
				cell = cell,
				worldIdx = worldIdx
			};

			if (!IsValidCell(cell, cellData))
				return;

			if (IsValidCell(cell, cellData))
				connectedCells.Add(cell);

			if (lastQueryCell != cell)
			{
				connectedCells = new();

				var cells = GameUtil.FloodCollectCells(cell, cell => IsValidCell(cell, cellData), 128);

				Log.Debug($"flood collectin from {cell}");
				connectedCells = cells
					.OrderBy(c => Grid.GetCellDistance(cell, c))
					.ToList();

				lastQueryCell = cell;
			}

			Debug.Log($"CELLS: {connectedCells.Count} {connectedCells.Join()}");
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

		private bool IsValidCell(int cell, CellData data)
		{
			if (data == null)
			{
				Log.Warning($"{nameof(Backwalls_SmartBuildTool)} {nameof(IsValidCell)} null data");
				return false;
			}

			return
				!connectedCells.Contains(cell)
				&& !Grid.Solid[cell]
				//&& BuildTool.Instance.def.IsValidPlaceLocation(BuildTool.Instance.def.BuildingPreview, cell, Orientation.Neutral, out var _)
				&& !Grid.Foundation[cell]
				&& !Grid.ObjectLayers[(int)ObjectLayer.Backwall].ContainsKey(cell)
				&& (data.cavity == null || Game.Instance.roomProber.GetCavityForCell(cell) == data.cavity)
				&& Grid.IsValidCellInWorld(cell, data.worldIdx);
		}

		void Update()
		{
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

				yield return new WaitForSecondsRealtime(0.2f);
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
		}

		public void OnKeyUp(KButtonEvent e)
		{
			if (e.IsAction(Action.MouseLeft))
			{
				Log.Debug("LEFT BUTTON UP");

				isMouseDown = false;
				isActive = false;
				lastQueryCell = -1;
				OverrideTargetCell = -1;
				alreadyUsedCells.Clear();
				StopCoroutine(TargetSmartCell());
			}
		}

		private bool IsEnabledBuilding(BuildingDef def)
		{
			return def != null && def.ObjectLayer == ObjectLayer.Backwall;
		}

		public class CellData
		{
			public int cell;
			public int worldIdx;
			public CavityInfo cavity;
		}
	}
}
