using ImGuiNET;
using Twitchery.Content.Scripts;

namespace Twitchery
{
    public class AETE_DevTool : DevTool
    {
        private static float pixelationAmount;

        public AETE_DevTool()
        {
            RequiresGameRunning = true;
        }

        public override void RenderTo(DevPanel panel)
        {
            var selected = SelectTool.Instance.selected;
            if(selected != null)
            {
                if (selected.TryGetComponent(out AETE_PolymorphCritter polymorphCritter))
                    polymorphCritter.OnImguiDebug();
            }

            if(ImGui.CollapsingHeader("Dithering"))
            {
                ImGui.DragFloat("Spread", ref AETE_DitherPostFx.Instance.fullyDitheredConfig.spread, 0.01f, 0f, 1f);
                ImGui.DragInt("Color Count", ref AETE_DitherPostFx.Instance.fullyDitheredConfig.colorCount);
                ImGui.DragInt("Down Samples", ref AETE_DitherPostFx.Instance.fullyDitheredConfig.downSamples, 1, 0, 8);
                ImGui.DragInt("Bayer Level", ref AETE_DitherPostFx.Instance.fullyDitheredConfig.bayerLevel, 1, 0, 3);

                if(ImGui.Button("Dither sequence"))
                {
                    AETE_DitherPostFx.Instance.DoDither();
                }

                if(ImGui.DragFloat("Dither", ref pixelationAmount, 0.01f, 0, 1))
                {
                    AETE_DitherPostFx.Instance.SetDitherExactly(pixelationAmount);
                }
            }
        }
    }
}
