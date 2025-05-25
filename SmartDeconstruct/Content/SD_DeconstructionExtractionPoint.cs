using System.Collections.Generic;
using UnityEngine;

namespace SmartDeconstruct.Content
{
	public class SD_DeconstructionExtractionPoint : KMonoBehaviour
	{
		public HashSet<int> connectedDeconstructables;
		public HashSet<int> queuedDeconstructables;
		[SerializeField] public ObjectLayer layer;

		public override void OnSpawn()
		{
			base.OnSpawn();
			connectedDeconstructables = [];
			var baseCell = Grid.PosToCell(this);
			GameUtil.FloodCollectCells(connectedDeconstructables, baseCell, cell => cell == baseCell || IsConnectableCell(baseCell));

			if (connectedDeconstructables.Count == 0)
			{
				Util.KDestroyGameObject(gameObject);
				return;
			}

			foreach (var cell in connectedDeconstructables)
			{
				if (SD_DeconstructablesManager.Instance.TryGetConnected(cell, layer, out SD_Deconstructable deconstructable))
				{
					deconstructable.MarkForDeconstruction(this);
				}
			}
			Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenu);
		}

		private void OnRefreshUserMenu(object obj)
		{
			var button = new KIconButtonMenu.ButtonInfo(
				"action_empty_contents",
				"Deconstruct Connected",
				DeconstructConnected,
				tooltipText: "Deconstuct all connected, making sure duplicants can always navigate back to this tile. This will be deconstructed last.");

			Game.Instance.userMenu.AddButton(gameObject, button);
		}

		private void DeconstructConnected()
		{
			FUtility.Utils.Spawn(SmartDeconstructMarkerConfig.ID, gameObject);
		}

		private bool IsConnectableCell(int cell)
		{
			return SD_DeconstructablesManager.Instance.TryGetConnected(cell, layer, out SD_Deconstructable deconstructable)
				&& deconstructable.extractionPointRef == null;
		}
	}
}
