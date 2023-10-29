using ProcGen;
using UnityEngine;

namespace Twitchery.Content.Scripts
{
	public class RegularPipBrain : Brain
	{
		public override void OnSpawn()
		{
			base.OnSpawn();
			foreach (var item in GetComponent<Storage>().items)
				AddAnimTracker(item);
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			//this.Navigator.SetAbilities((PathFinderAbilities)new MinionPathFinderAbilities(this.Navigator));
			Subscribe((int)GameHashes.OnStorageChange, AnimTrackStoredItem);
		}

		public override void UpdateBrain()
		{
			base.UpdateBrain();

			if (Game.Instance == null)
				return;

			CheckSpaceDiscovery();
			CheckOilDiscovery();
		}

		private void CheckOilDiscovery()
		{
			if (!Game.Instance.savedInfo.discoveredOilField
				&& World.Instance.zoneRenderData.GetSubWorldZoneType(Grid.PosToCell(gameObject)) == SubWorld.ZoneType.OilField)
				Game.Instance.savedInfo.discoveredOilField = true;
		}

		private void AnimTrackStoredItem(object data)
		{
			var storage = GetComponent<Storage>();
			var go = (GameObject)data;

			RemoveTracker(go);

			if (!storage.items.Contains(go))
				return;

			AddAnimTracker(go);
		}

		private void AddAnimTracker(GameObject go)
		{
			var controller = go.GetComponent<KAnimControllerBase>();

			if (controller == null
				|| controller.AnimFiles == null
				|| controller.AnimFiles.Length == 0
				|| controller.AnimFiles[0] == null
				|| !controller.GetComponent<Pickupable>().trackOnPickup)
				return;

			var tracker = go.AddComponent<KBatchedAnimTracker>();
			tracker.useTargetPoint = false;
			tracker.fadeOut = false;
			tracker.symbol = new HashedString("sq_body");
			tracker.forceAlwaysVisible = true;
		}

		private void RemoveTracker(GameObject go)
		{
			if (go.TryGetComponent(out KBatchedAnimTracker tracker))
				Destroy(tracker);
		}

		private void CheckSpaceDiscovery()
		{
			if (!Game.Instance.savedInfo.discoveredSurface
				&& World.Instance.zoneRenderData.GetSubWorldZoneType(Grid.PosToCell(gameObject)) == SubWorld.ZoneType.Space)
			{
				Game.Instance.savedInfo.discoveredSurface = true;
				var discoveredSpaceMessage = new DiscoveredSpaceMessage(this.gameObject.transform.GetPosition());
				Messenger.Instance.QueueMessage(discoveredSpaceMessage);
				Game.Instance.Trigger((int)GameHashes.DiscoveredSpace, gameObject);
			}
		}
	}
}
