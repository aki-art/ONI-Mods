using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events
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
			tree.minDistance = 10;
			tree.minTimeDelay = Random.Range(6, 12);

			go.SetActive(true);
		}
	}
}
