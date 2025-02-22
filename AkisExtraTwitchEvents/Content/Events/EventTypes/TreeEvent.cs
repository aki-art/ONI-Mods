using ONITwitchLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Content.Events.EventTypes
{
	public class TreeEvent() : TwitchEventBase(ID)
	{
		public const string ID = "Tree";

		public override int GetWeight() => Consts.EventWeight.Common;

		public override Danger GetDanger() => Danger.Medium;

		public override void Run()
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
