using FUtility;
using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackB.Content.Scripts
{
	public class FoundationOrAnchorMonitor : KMonoBehaviour
	{
		[SerializeField] public CellOffset[] monitorCells;

		[Serialize] public bool needsFoundation = true;
		[Serialize] public bool hasFoundation = true;

		[MyCmpReq] private Operational operational;
		[MyCmpReq] private KSelectable kSelectable;

		public static readonly Operational.Flag foundationFlag = new("decorpackb_requires_foundation", Operational.Flag.Type.Functional);

		private List<HandleVector<int>.Handle> partitionerEntries = new();
		private int position;

		protected override void OnSpawn()
		{
			base.OnSpawn();
			position = Grid.PosToCell(gameObject);

			Subscribe(DPIIHashes.FossilStageSet, OnFossilStageSet);
			Subscribe(DPIIHashes.FossilStageUnset, OnFossilStageUnset);
		}

		private void OnFossilStageUnset(object obj)
		{
			needsFoundation = false;
		}

		private void OnFossilStageSet(object _)
		{
			needsFoundation = true;
			UpdateCells();
		}

		private void UpdateCells()
		{
			if (partitionerEntries != null)
			{
				for (int i = 0; i < partitionerEntries.Count; i++)
				{
					var partitionerEntry = partitionerEntries[i];
					GameScenePartitioner.Instance.Free(ref partitionerEntry);
				}
			}

			foreach (var monitorCell in monitorCells)
			{
				var cell = Grid.OffsetCell(position, monitorCell);
				if (Grid.IsValidCell(position) && Grid.IsValidCell(cell))
					partitionerEntries.Add(
						GameScenePartitioner.Instance.Add("FoundationOrAnchorMonitor.UpdateCells",
						gameObject, cell,
						GameScenePartitioner.Instance.solidChangedLayer,
						OnGroundChanged));

				OnGroundChanged(null);
			}
		}

		public bool CheckFoundationValid() => !needsFoundation || IsSuitableFoundation(position);

		public bool IsSuitableFoundation(int cell)
		{
			foreach (var monitorCell in monitorCells)
			{
				if (!Grid.IsCellOffsetValid(cell, monitorCell))
					return false;

				var i = Grid.OffsetCell(cell, monitorCell);

				if (!Grid.Solid[i])
					return false;
			}

			return true;
		}

		public void OnGroundChanged(object callbackData)
		{
			var foundationValid = CheckFoundationValid();

			if (!hasFoundation && foundationValid)
				hasFoundation = true;
			else if (hasFoundation && !foundationValid)
				hasFoundation = false;

			operational.SetFlag(foundationFlag, hasFoundation);
			UpdateStatusItem();
		}

		private void UpdateStatusItem()
		{
			kSelectable.ToggleStatusItem(Db.Get().BuildingStatusItems.MissingFoundation, hasFoundation, this);
		}

		public void RecalculateCeilingCells(float[] ceilingDistances)
		{
			Log.Debug("recalculating ceiling cells");

			for (int i = 0; i < ceilingDistances.Length; i++)
			{
				var dist = Mathf.FloorToInt(ceilingDistances[i]);
				monitorCells[i] = new CellOffset(i, dist);
				Log.Debug($"   {i}: {dist}");
			}

			UpdateCells();
		}
	}
}
