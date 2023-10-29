using HarmonyLib;
using Klei;

namespace Twitchery.Patches
{
	public class GameFlowManagerPatch
	{
		[HarmonyPatch(typeof(GameFlowManager.StatesInstance), "CheckForGameOver")]
		public class GameFlowManager_StatesInstance_CheckForGameOver_Patch
		{
			public static bool Prefix()
			{
				if (!Game.Instance.GameStarted() || GenericGameSettings.instance.disableGameOver)
					return true;

				if (Mod.Settings.SuppressColonyLostMessage)
				{
					if (Mod.polys.Count > 0
						|| Mod.midasContainersWithDupes.Count > 0
						|| Mod.regularPips.Count > 0
						|| Mod.wereVoles.Count > 0)
						return false;
				}

				return true;
			}
		}
	}
}
