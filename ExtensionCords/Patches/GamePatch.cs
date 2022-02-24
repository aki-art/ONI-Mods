using HarmonyLib;

namespace ExtensionCords.Patches
{
    internal class GamePatch
    {
        [HarmonyPatch(typeof(Game), "DestroyInstances")]
        public class Game_DestroyInstances_Patch
        {
            public static void Postfix()
            {
                ReelTool.DestroyInstance();
            }
        }
    }
}
