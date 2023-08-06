using ONITwitchLib;
using Twitchery.Content.Defs;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events
{
	internal class TreeEvent : ITwitchEvent
	{
		public const string ID = "Tree";

		public bool Condition(object data) => true;

		public string GetID() => ID;

		public void Run(object data)
		{
			var go = new GameObject("tree spawner");
			var tree = go.AddOrGet<TreeSpawner>();
			tree.minDistance = 15;
			tree.minTimeDelay = Random.Range(10, 20);

			go.SetActive(true);
		}
	}
}
