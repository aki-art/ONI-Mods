using DecorPackB.Content.Scripts;
using UnityEngine;

namespace DecorPackB.Integration.Twitch
{
	public class LuckyPotsCommand
	{
		public const string ID = "LuckyPots";

		public static bool Condition(object _) => true;

		public static void Run(object _)
		{
			var go = new GameObject("Lucky Pot Spawner");

			var twitchLuckyPotSpawner = go.AddComponent<TwitchLuckyPotSpawner>();
			twitchLuckyPotSpawner.duration = 20f;
			twitchLuckyPotSpawner.radius = 4;

			go.SetActive(true);
		}
	}
}
