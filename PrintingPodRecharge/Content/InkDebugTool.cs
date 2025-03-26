#if DEVTOOLS
using FUtility;
using ImGuiNET;
using PrintingPodRecharge.Content.Cmps;
using System;
using System.Collections.Generic;

namespace PrintingPodRecharge.Content
{
	public class InkDebugTool : DevTool
	{
		private readonly string[] bundles;
		private int selectedBundle;

		public InkDebugTool()
		{
			bundles = Enum.GetNames(typeof(Bundle));
			if (!DlcManager.IsContentSubscribed(CONSTS.DLC_BIONIC))
			{
				var bundlesList = new List<string>(bundles);
				bundlesList.Remove(Bundle.Bionic.ToString());
				bundles = bundlesList.ToArray();
			}
		}

		public override void RenderTo(DevPanel panel)
		{
			ImGui.Text("Prints");
			ImGui.Spacing();
			ImGui.Combo("Bundles:", ref selectedBundle, bundles, bundles.Length);

			if (ImmigrationModifier.Instance != null)
			{
				if (ImGui.Button("Set Bundle"))
					ImmigrationModifier.Instance.SetModifier((Bundle)selectedBundle);

				if (ImGui.Button($"Force Print {(Bundle)selectedBundle}"))
				{
					ImmigrationModifier.Instance.SetModifier((Bundle)selectedBundle);

					ImmigrantScreen.InitializeImmigrantScreen(GameUtil.GetActiveTelepad().GetComponent<Telepad>());
					Game.Instance.Trigger((int)GameHashes.UIClear);
				}
			}

			if (ImGui.Button("Start Print"))
				Immigration.Instance.timeBeforeSpawn = 0;

			ImGui.Separator();

			if (BioInksD6Manager.Instance != null)
			{
				ImGui.Text("D6 count: " + BioInksD6Manager.Instance.diceCount);

				if (ImGui.Button("Add D6"))
					BioInksD6Manager.Instance.AddDie(1);

				if (ImGui.Button("Add 1000 D6"))
					BioInksD6Manager.Instance.AddDie(1000);
			}
		}
	}
}
#endif