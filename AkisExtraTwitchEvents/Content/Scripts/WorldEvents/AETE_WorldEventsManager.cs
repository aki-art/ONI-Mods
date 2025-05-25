using KSerialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Twitchery.Content.Scripts.WorldEvents
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class AETE_WorldEventsManager : KMonoBehaviour
	{
		public static AETE_WorldEventsManager Instance { get; set; }

		public Dictionary<byte, WorldData> eventsPerWorlds;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Instance = this;
		}

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Instance = null;
		}

		public AETE_WorldEvent CreateEvent(string id, bool bigEvent)
		{
			var destination = FindEligibleWorldForEvent(id, bigEvent, true);

			Log.Debug($"spawning event at: {destination}");
			if (destination != -1)
			{
				var world = ClusterManager.Instance.GetWorld(destination);
				if (world == null)
					return null;

				return CreateEvent
					(id, bigEvent, (Vector3)(world.minimumBounds + world.WorldSize / 2));
			}

			return null;
		}

		public AETE_WorldEvent CreateEvent(string id, bool bigEvent, Vector3 position)
		{
			var worldIdx = Grid.WorldIdx[Grid.PosToCell(position)];
			CheckWorldIn(worldIdx);

			if (!eventsPerWorlds[worldIdx].CanHostEvent(id, bigEvent))
				return null;

			var go = FUtility.Utils.Spawn(id, position);
			return go.GetComponent<AETE_WorldEvent>();
		}

		public void OnEventEnd(AETE_WorldEvent worldEvent)
		{
			if (eventsPerWorlds == null || worldEvent == null)
				return;

			eventsPerWorlds[(byte)worldEvent.GetMyWorldId()]?.Remove(worldEvent);
		}

		public void OnEventBegin(AETE_WorldEvent worldEvent)
		{
			Log.Debug($"On event begin: {worldEvent.PrefabID()}");
			var worldIdx = (byte)worldEvent.GetMyWorldId();
			CheckWorldIn(worldIdx);

			eventsPerWorlds[worldIdx].Add(worldEvent);
		}

		private void CheckWorldIn(byte worldIdx)
		{
			eventsPerWorlds ??= [];

			if (!eventsPerWorlds.ContainsKey(worldIdx))
				eventsPerWorlds.Add(worldIdx, new WorldData(worldIdx));
		}

		public bool CanStartNewEvent(string eventId, bool bigEvent) => FindEligibleWorldForEvent(eventId, bigEvent, false) != -1;

		private int FindEligibleWorldForEvent(string eventId, bool bigEvent, bool checkActiveWorldFirst)
		{
			if (checkActiveWorldFirst
				&& ClusterManager.Instance.activeWorld != null
				&& CanWorldStartEvent(eventId, bigEvent, ClusterManager.Instance.activeWorld))
				return ClusterManager.Instance.activeWorld.id;

			foreach (var world in ClusterManager.Instance.WorldContainers)
			{
				if (CanWorldStartEvent(eventId, bigEvent, world))
					return world.id;
			}

			Log.Debug("-1");

			return -1;
		}

		private bool CanWorldStartEvent(string eventId, bool bigEvent, WorldContainer world)
		{
			if (world.IsModuleInterior)
				return false;

			CheckWorldIn((byte)world.id);

			return eventsPerWorlds[(byte)world.id].CanHostEvent(eventId, bigEvent)
				&& Components.LiveMinionIdentities.GetWorldItems(world.id, false).Count > 0;
		}

		public void EndAllEvents()
		{
			if (eventsPerWorlds == null)
				return;

			var list = new List<AETE_WorldEvent>();

			foreach (var world in eventsPerWorlds)
			{
				var events = world.Value.onGoingEvents;
				if (events == null)
					continue;

				foreach (var @event in events)
				{
					if (@event != null)
						list.Add(@event);
				}
			}

			for (int i = list.Count - 1; i >= 0; i--)
				list[i].End();
		}

		public class WorldData(byte worldIdx)
		{
			public byte worldIdx = worldIdx;
			public readonly List<AETE_WorldEvent> onGoingEvents = [];

			public bool HasAnyEvent() => onGoingEvents.Count > 0;

			public bool HasExclusiveEvent() => onGoingEvents.Any(e => e.bigEvent);

			public bool CanHostEvent(string id, bool bigEvent)
			{
				ClearInvalidEvents();

				if (onGoingEvents.Count == 0)
					return true;

				if (HasExclusiveEvent() && bigEvent)
					return false;

				if (onGoingEvents.Any(e => e.IsPrefabID(id)))
					return false;

				return true;
			}

			private void ClearInvalidEvents()
			{
				onGoingEvents.RemoveAll(e => e == null);
			}

			public void Add(AETE_WorldEvent worldEvent) => onGoingEvents.Add(worldEvent);

			public void Remove(AETE_WorldEvent worldEvent) => onGoingEvents.Remove(worldEvent);

			public bool TryGetWorldContainer(out WorldContainer container)
			{
				container = ClusterManager.Instance.GetWorld(worldIdx);
				return container != null;
			}
		}
	}
}
