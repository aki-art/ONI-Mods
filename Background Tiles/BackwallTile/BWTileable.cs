using FUtility;
using UnityEngine;

namespace BackgroundTiles.BackwallTile
{
    internal class BWTileable : KMonoBehaviour
	{
		private Extents buildingExtends;
		private HandleVector<int>.Handle partitionerEntry;
		public ObjectLayer objectLayer = ObjectLayer.Backwall;

		[MyCmpGet]
		private KBatchedAnimController kbac;

		private static KAnimHashedString l = new KAnimHashedString("l");
		private static KAnimHashedString bl = new KAnimHashedString("bl");
		private static KAnimHashedString tl = new KAnimHashedString("tl");
		private static KAnimHashedString r = new KAnimHashedString("r");
		private static KAnimHashedString br = new KAnimHashedString("br");
		private static KAnimHashedString tr = new KAnimHashedString("tr");
		private static KAnimHashedString t = new KAnimHashedString("t");
		private static KAnimHashedString b = new KAnimHashedString("b");

		protected override void OnSpawn()
		{
			buildingExtends = GetComponent<Building>().GetExtents();

			Extents extents = new Extents(buildingExtends.x - 1, buildingExtends.y - 1, buildingExtends.width + 2, buildingExtends.height + 2);

			partitionerEntry = GameScenePartitioner.Instance.Add("BWTileable.OnSpawn", gameObject, extents, GameScenePartitioner.Instance.objectLayers[(int)objectLayer], OnNeighbourCellsUpdated);
			UpdateEndCaps();
		}

		protected override void OnCleanUp()
		{
			GameScenePartitioner.Instance.Free(ref partitionerEntry);
			base.OnCleanUp();
		}

		private void UpdateEndCaps()
		{
			int cell = Grid.PosToCell(this);

			bool top = !HasTileableNeighbour(Grid.CellAbove(cell));
			bool bottom = !HasTileableNeighbour(Grid.CellBelow(cell));
			bool left = !HasTileableNeighbour(Grid.CellLeft(cell));
			bool right = !HasTileableNeighbour(Grid.CellRight(cell));

			Log.Debuglog("UPDATING: ", top, bottom, left, right);
			/*
			kbac.SetSymbolVisiblity(t, top);
			kbac.SetSymbolVisiblity(tl, top && left);
			kbac.SetSymbolVisiblity(tr, top && right);
			kbac.SetSymbolVisiblity(b, bottom);
			kbac.SetSymbolVisiblity(bl, bottom && left);
			kbac.SetSymbolVisiblity(br, bottom && right);
			kbac.SetSymbolVisiblity(l, left);
			kbac.SetSymbolVisiblity(r, right);
			*/


			kbac.SetSymbolVisiblity(t, false);
			kbac.SetSymbolVisiblity(tl, false);
			kbac.SetSymbolVisiblity(tr, false);
			kbac.SetSymbolVisiblity(b, false);
			kbac.SetSymbolVisiblity(bl, false);
			kbac.SetSymbolVisiblity(br, false);
			kbac.SetSymbolVisiblity(l, false);
			kbac.SetSymbolVisiblity(r, false);
		}

		private void OnNeighbourCellsUpdated(object data)
		{
			if (this is null || gameObject is null || !partitionerEntry.IsValid())
			{
				return;
			}

			UpdateEndCaps();
		}

		private bool HasTileableNeighbour(int cell)
		{
			GameObject gameObject = Grid.Objects[cell, (int)objectLayer];
			return gameObject != null && gameObject.TryGetComponent(out BWTileable _);
		}
	}
}
