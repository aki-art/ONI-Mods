using UnityEngine;

namespace DecorPackA.Integration.Twitch
{
	public class FloorUpgradeCommand
	{
		public const string ID = "FloorUpgrade";

		public static bool Condition(object _) => FloorUpgrader.FindEligibleRoom() != null;

		public static void Run(object _)
		{
			var go = new GameObject("Floor Upgrader");
			go.AddComponent<FloorUpgrader>();
			go.SetActive(true);
		}
	}
}
