using Backwalls.Cmps;
using ImGuiNET;

namespace Backwalls
{
    public class BackwallsDevtool : DevTool
    {
        private static float z = Grid.GetLayerZ(Grid.SceneLayer.Backwall);
        private static int renderQueue = 3000;
        private static bool zWrite = true;

        protected override void RenderTo(DevPanel panel)
        {
            ImGui.DragFloat("Z layer", ref z);
            ImGui.DragInt("Render Queue", ref renderQueue);
            ImGui.Checkbox("ZWrite", ref zWrite);

            if(ImGui.Button("Apply"))
            {
                Mod.renderer.DebugForceRebuild(z, renderQueue, zWrite ? 1 : 0);
            }
        }
    }
}
