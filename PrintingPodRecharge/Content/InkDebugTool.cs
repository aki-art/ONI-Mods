using FUtility;
using ImGuiNET;
using PrintingPodRecharge.Content.Cmps;
using System;

namespace PrintingPodRecharge.Content
{
    public class InkDebugTool : DevTool
    {
        private readonly string[] bundles;
        private int selectedBundle;

        public InkDebugTool()
        {
            bundles = Enum.GetNames(typeof(Bundle));
        }

        protected override void RenderTo(DevPanel panel)
        {
            ImGui.Text("Prints");
            ImGui.Spacing();
            ImGui.Combo("Bundles:", ref selectedBundle, bundles, bundles.Length);
            
            if(ImmigrationModifier.Instance != null)
            {
                if (ImGui.Button("Set Bundle"))
                {
                    ImmigrationModifier.Instance.SetModifier((Bundle)selectedBundle);
                }

                if (ImGui.Button($"Force Print {(Bundle)selectedBundle}"))
                {
                    ImmigrationModifier.Instance.SetModifier((Bundle)selectedBundle);

                    ImmigrantScreen.InitializeImmigrantScreen(GameUtil.GetActiveTelepad().GetComponent<Telepad>());
                    Game.Instance.Trigger((int)GameHashes.UIClear);
                }
            }

            if (ImGui.Button("Start Print"))
            {
                Immigration.Instance.timeBeforeSpawn = 0;
            }

            ImGui.Spacing();

            if(BioInksD6Manager.Instance != null)
            {
                ImGui.Text("D6 count: " + BioInksD6Manager.Instance.diceCount);
                if (ImGui.Button("Add D6"))
                {
                    Log.Assert("BioInksD6Manager.Instance", BioInksD6Manager.Instance);
                    BioInksD6Manager.Instance.AddDie(1);
                }
            }

#if TWITCH
            ImGui.TextColored(new Vector4(1f, 0.3f, 1f, 1f), "Twitch Integration");
            ImGui.Checkbox("Floor Upgrader testmode on", ref FloorUpgrader.testingMode);
#endif
        }
    }
}