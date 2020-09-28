/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InteriorDecorationVolI.Buildings.Aquarium
{
    public class FishTank : SingleEntityReceptacle
	{
		private int HP = 100;
		private List<FetchOrder2> fetches;
		public CellOffset[] deliveryOffsets = new CellOffset[1];
		public CellOffset spawnOffset = new CellOffset(0, 0);
		public Storage waterStorage;
		public Storage creatureStorage;
		private FishTankStates.Instance smi;

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			fetches = new List<FetchOrder2>();
			autoReplaceEntity = true;
			occupyingObjectRelativePosition = new Vector3(0.5f, 1f, -1f);

		}
		protected override void OnSpawn()
		{
			base.OnSpawn();
			smi = new FishTankStates.Instance(this);
			smi.StartSM();

			foreach(var storage in GetComponents<Storage>())
			{
				if (storage.storageFilters.Contains(SimHashes.Water.CreateTag()))
					waterStorage = storage;
				else creatureStorage = storage;
			}

		}
	}
}
*/