using KSerialization;
using System;
using System.Collections.Generic;

namespace SmartDeconstruct
{
	public class SD_DeconstructablesManager : KMonoBehaviour
	{
		public static Dictionary<ObjectLayer, Dictionary<int, SD_Deconstructable>> cachedDeconstructablesPerCell;

		[Serialize] public Dictionary<int, List<Ref<SD_Deconstructable>>> deconstructableRefs;
		public static int[] networks;
		public static SD_DeconstructablesManager Instance { get; set; }

		public override void OnPrefabInit()
		{
			Instance = this;
		}

		public override void OnCleanUp()
		{
			Instance = null;
		}

		public override void OnSpawn()
		{
			networks = new int[Grid.CellCount];
			cachedDeconstructablesPerCell = [];
		}

		public void Add(int network, SD_Deconstructable deconstructable)
		{
			deconstructableRefs ??= [];

			if (!deconstructableRefs.ContainsKey(network))
				deconstructableRefs.Add(network, []);

			deconstructableRefs[network].Add(new(deconstructable));

			networks ??= new int[Grid.CellCount];
			networks[Grid.PosToCell(deconstructable)] = network;

			UpdateNetwork(network);
		}

		private void UpdateNetwork(int network)
		{
		}

		public void MergeNetworks(int a, int b)
		{

		}

		public bool TryGetNetworkId(int cell, out int networkId)
		{
			networkId = -1;
			if (!Grid.IsValidCell(cell))
				return false;

			networkId = networks[cell];
			return true;
		}

		public void Remove(int network, SD_Deconstructable deconstructable)
		{
			if (deconstructableRefs == null || !deconstructableRefs.ContainsKey(network))
			{
				Log.Warning($"Trying to remove deconstructable connection, but the attached network ID does not exist: {network}");
				return;
			}

			if (deconstructable.IsNullOrDestroyed())
			{
				Log.Warning("Trying to remove deconstructable from network, but it was null.");
				return;
			}

			var instanceId = deconstructable.GetComponent<KPrefabID>().InstanceID;

			networks[Grid.PosToCell(deconstructable)] = 0;

			deconstructableRefs[network].RemoveAll(r => r.id == instanceId);
			UpdateNetwork(network);
		}

		internal int GetNewNetworkId(int offsetCell)
		{
			throw new NotImplementedException();
		}

		public bool TryGetConnected(int cell, ObjectLayer layer, out SD_Deconstructable deconstructable)
		{
			deconstructable = null;
			if (cachedDeconstructablesPerCell.TryGetValue(layer, out var deconsts))
			{
				if (deconsts != null && deconsts.TryGetValue(cell, out deconstructable))
					return !deconstructable.IsNullOrDestroyed();
			}

			return false;
		}
	}
}
