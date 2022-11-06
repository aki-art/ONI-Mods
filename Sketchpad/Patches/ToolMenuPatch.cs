using HarmonyLib;
using SketchPad.Tools.Pencil;

namespace Sketchpad.Patches
{
    public class ToolMenuPatch
    {
        [HarmonyPatch(typeof(ToolMenu), "CreateBasicTools")]
        public static class ToolMenuCreateBasicTools
        {
            public static void Prefix(ToolMenu __instance)
            {
                __instance.basicTools.Add(ToolMenu.CreateToolCollection(
                    "Pencil",
                    "icon_errand_art",
                    Action.BuildMenuKeyP,
                    PencilTool.NAME,
                    "tooltip {HotKey}",
                    false
                ));
            }
        }
    }
}
