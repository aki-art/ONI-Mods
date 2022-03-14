using HarmonyLib;
using SketchPad.History;
using SketchPad.Tools.Pencil;

namespace SketchPad.Patches
{
    internal class GamePatch
    {

        [HarmonyPatch(typeof(Game), "OnPrefabInit")]
        public class Game_OnPrefabInit_Patch
        {
            public static void Postfix()
            {
                Game.Instance.gameObject.AddComponent<PencilTool>();
                Game.Instance.gameObject.AddComponent<EditHistory>();
            }
        }
    }
}
