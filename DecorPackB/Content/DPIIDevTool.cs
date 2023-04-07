using DecorPackB.Content.Scripts;
using ImGuiNET;

namespace DecorPackB.Content
{
    public class DPIIDevTool : DevTool
    {
        // use empty constructor or the game will try to automatically add this after scraping for all devtool classes, and then get confused
        public DPIIDevTool()
        {
        }

        private static bool functionalFossils;
        private static bool archeology;
        private static bool fossilio;

        protected override void RenderTo(DevPanel panel)
        {
            ImGui.Checkbox("Functional fossils", ref functionalFossils);

        }
    }
}
