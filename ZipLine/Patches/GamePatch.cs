using HarmonyLib;
using ZipLine.Content.Tools;

namespace ZipLine.Patches
{
    public class GamePatch
    {
        [HarmonyPatch(typeof(Game), "DestroyInstances")]
        public class Game_DestroyInstances_Patch
        {
            public static void Postfix()
            {
                ZipConnectorTool.DestroyInstance();
            }
        }
    }
}
