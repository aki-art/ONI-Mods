using HarmonyLib;

namespace TrueTiles.Patches
{
	public class GamePatch
	{
		// needs to be manually patched, otherwise it loads Game type too early, which in turns CustomGameSettings, and breaks some translations on it
		public class Game_OnSpawn_Patch
		{
			public static void Patch(Harmony harmony)
			{
				var m_OnSpawn = AccessTools.Method("Game, Assembly-CSharp:OnSpawn");
				var m_Postfix = AccessTools.Method(typeof(Game_OnSpawn_Patch), "Postfix");
				harmony.Patch(m_OnSpawn, null, new HarmonyMethod(m_Postfix));
			}

			public static void Postfix()
			{
				ElementGrid.Initialize();
			}
		}
	}
}
