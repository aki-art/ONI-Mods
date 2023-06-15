/*using UnityEngine;

namespace PrintingPodRecharge.Integration.TwitchIntegration
{
	public class WackyDupeCommand
	{
		public const string ID = "WackyDupe";

		public static bool Condition() => true;

		public static void Run(object data)
		{
			SpawnMinion();
		}

		public static Color GetRandomHairColor()
		{
			return Random.ColorHSV(0, 1, 0.5f, 1f, 0.4f, 1f);
		}

		private static void SpawnMinion()
		{
			var prefab = Assets.GetPrefab(MinionConfig.ID);
			var gameObject = Util.KInstantiate(prefab, null, prefab.name);

			Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);

			var position = Grid.CellToPosCBC(Grid.PosToCell(GameUtil.GetActiveTelepad()), Grid.SceneLayer.Move);
			gameObject.transform.SetLocalPosition(position);
			gameObject.SetActive(true);

			var stats = new MinionStartingStats(false);
			var data = DupeGenHelper.GenerateRandomDupe(stats);

			if (!Mod.otherMods.IsMeepHere || Mod.Settings.ColoredMeeps)
				data.hairColor = GetRandomHairColor(); // more saturated wackier hairs

			DupeGenHelper.Wackify(stats, gameObject);

			stats.Apply(gameObject);

			ONITwitchLib.ToastManager.InstantiateToastWithGoTarget("Spawning Duplicant", $"{gameObject.GetProperName()} has been brought into the world!", gameObject);
		}
	}
}
*/