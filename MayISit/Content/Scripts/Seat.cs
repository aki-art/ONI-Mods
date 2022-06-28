using UnityEngine;

namespace MayISit.Content.Scripts
{
    public class Seat : KMonoBehaviour, IApproachable, ISim200ms
    {
        [SerializeField]
        public float workTime = 5f;

		public bool choresDirty;
		public int choreCount = 2;

		public string specificEffect = ModDb.Effects.LOUNGED;
		public string trackingEffect = ModDb.Effects.RECENTLY_LOUNGED;

		// One for each sittable position
		[SerializeField]
        public CellOffset[] seatOffsets = new CellOffset[]
        {
            new CellOffset(0, 0)
        };

		private Chore[] chores;

		public int GetCell()
		{
			return Grid.PosToCell(this);
		}

		public CellOffset[] GetOffsets()
        {
			return seatOffsets;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            chores = new Chore[seatOffsets.Length];
        }

		public void UpdateSitChores(bool force = true)
		{
			if (!force && !choresDirty)
			{
				return;
			}

			int choresCounter = 0;

			for (int i = 0; i < seatOffsets.Length; i++)
			{
				var offset = seatOffsets[i];
				var chore = chores[i];

				if (choresCounter < choreCount && IsOffsetValid(offset))
				{
					choresCounter++;

					if (chore == null || chore.isComplete)
					{
						chores[i] = new SitChore(this, OnChoreEnd);
					}
				}
				else if (chore != null)
				{
					chore.Cancel("invalid");
					chores[i] = null;
				}
			}

			choresDirty = false;
		}

		public void CancelSitChores()
		{
			for (int i = 0; i < seatOffsets.Length; i++)
			{
				var chore = chores[i];
				if (chore != null)
				{
					chore.Cancel("cancelled");
					chores[i] = null;
				}
			}
		}

		private void OnCellChanged(object data)
		{
			choresDirty = true;
		}

		private void OnChoreEnd(Chore chore)
		{
			choresDirty = true;
		}

		private bool IsOffsetValid(CellOffset offset)
		{
			var cell = Grid.OffsetCell(Grid.PosToCell(this), offset);
			var anchor_cell = Grid.CellBelow(cell);

			return GameNavGrids.FloorValidator.IsWalkableCell(cell, anchor_cell, false);
		}

        public void Sim200ms(float dt)
        {
			UpdateSitChores(true);
        }
    }
}
