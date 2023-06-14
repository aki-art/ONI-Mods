using UnityEngine;

namespace DecorPackA.Integration.Twitch
{
	public class FloorUpgradeCommand
	{
		public const string ID = "FloorUpgrade";

		public static bool Condition() => true;

		public static void Run(object data)
		{
			var go = new GameObject("Floor Upgrader");
			go.AddComponent<FloorUpgrader>();
			go.SetActive(true);
		}
	}
}
