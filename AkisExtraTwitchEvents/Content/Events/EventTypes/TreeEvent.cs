using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class TreeEvent : ITwitchEvent
	{
		public const string ID = "Tree";

		public int GetWeight() => TwitchEvents.Weights.COMMON;

		public bool Condition(object data) => true;

		public string GetID() => ID;

		public void Run(object data)
		{
			var go = new GameObject("tree spawner");
			var tree = go.AddOrGet<TreeSpawner>();
			tree.minDistance = 23;
			tree.minTimeDelay = Random.Range(15, 45);
			tree.minAmount = 1;
			tree.maxAmount = 3;

			go.SetActive(true);
		}
	}
}
