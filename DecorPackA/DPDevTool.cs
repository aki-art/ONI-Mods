using DecorPackA.Buildings.GlassSculpture;
using ImGuiNET;
using System;
using UnityEngine;

namespace DecorPackA
{
/*	public class DPDevTool : DevTool
	{
		private static float percent;

		public DPDevTool() => RequiresGameRunning = true;

		public override void RenderTo(DevPanel panel)
		{
			if (SelectTool.Instance.selected != null
				&& SelectTool.Instance.selected.TryGetComponent(out KBatchedAnimController kbac))
			{
				if (ImGui.DragFloat("Position Percent", ref percent))
				{
					kbac.SetPositionPercent(percent);
				}

				ImGui.Text("elapsed: " + kbac.elapsedTime);
				ImGui.Text("numFrames: " + kbac.curAnim.numFrames);
				ImGui.Text("frameRate: " + kbac.curAnim.frameRate);
				ImGui.Text("currentFrame: " + kbac.currentFrame);
				ImGui.Text("(float)curAnim.numFrames / curAnim.frameRate * percent: " + ((float)kbac.curAnim.numFrames / kbac.curAnim.frameRate * percent));
				ImGui.Text("frameIdx: " + kbac.curAnim.GetFrameIdx(kbac.mode, kbac.elapsedTime));

			}
		}
	}*/
}
