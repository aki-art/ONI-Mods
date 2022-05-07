using HarmonyLib;

namespace TrueTiles.Patches
{
    public class GamePatch
    {
        [HarmonyPatch(typeof(Game), "OnSpawn")]
        public class Game_OnSpawn_Patch
        {
            public static void Postfix()
            {
                ElementGrid.Initialize();
                FUtility.Log.Debuglog("GAME ONSPAWN");
            }
        }
    }
}
