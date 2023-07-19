using ImGuiNET;
using Moonlet.Content.Tools;
using Moonlet.ZoneTypes;
using System.Linq;
using UnityEngine;

namespace Moonlet.MoonletDevTools
{
	public class ZoneTypeDevTool : DevTool
	{
		private string[] zoneTypes;
		private static Vector4 zoneTypeColor;
		private static Vector4 previousZoneTypeColor;
		private static int selectedZoneType;

		public override void RenderTo(DevPanel panel)
		{
			ImGui.Text("Preview colors of a zone type.");

			if (zoneTypes == null || zoneTypes.Length == 0)
				zoneTypes = ZoneTypeUtil.zones.Select(z => z.id).ToArray();

			ImGui.ListBox("ZoneType", ref selectedZoneType, zoneTypes, zoneTypes.Length);

			ImGui.SetColorEditOptions(ImGuiColorEditFlags.DisplayHex);
			ImGui.ColorPicker4("Color", ref zoneTypeColor);

			if (zoneTypeColor != previousZoneTypeColor)
			{
				if (zoneTypeColor != null)
				{
					var color = new Color(zoneTypeColor.x, zoneTypeColor.y, zoneTypeColor.z, zoneTypeColor.w);

					World.Instance.zoneRenderData.zoneColours[(int)ZoneTypeUtil.zones.ElementAt(selectedZoneType).type] = color;
					World.Instance.zoneRenderData.OnActiveWorldChanged();

					previousZoneTypeColor = zoneTypeColor;
				}
			}

			if (ImGui.SmallButton("Copy Hex to clipboard"))
			{
				var color = (Color)zoneTypeColor;
				ImGui.SetClipboardText(color.ToHexString());
			}

			ImGui.Separator();

			if (ImGui.Button("Paint"))
				Moonlet_ZonetypePainterTool.Instance.Activate();
		}
	}
}
