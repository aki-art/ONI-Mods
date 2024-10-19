using Moonlet.Patches;
using System.Collections.Generic;
using UnityEngine;
using static ProcGen.SubWorld;

namespace Moonlet.Scripts.Tools
{
	public class Moonlet_ZonetypePainterTool : BrushTool
	{
		public static Moonlet_ZonetypePainterTool Instance { get; private set; }
		public ZoneType targetZoneType = ZoneType.Sandstone;
		protected Color recentlyAffectedCellColor = new(1f, 1f, 1f, 0.1f);

		private SandboxSettings settings => SandboxToolParameterMenu.instance.settings;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Instance = this;
		}

		public override void OnSpawn()
		{
			base.OnSpawn();
			Moonlet_Mod.Instance.Subscribe(ModEvents.onSandboxZoneTypeChanged, value => targetZoneType = (ZoneType)value);
		}

		public void Activate() => PlayerController.Instance.ActivateTool(this);

		public override void OnCleanUp()
		{
			base.OnCleanUp();
			Instance = null;
		}

		public override void OnActivateTool()
		{
			base.OnActivateTool();
			SandboxToolParameterMenu.instance.gameObject.SetActive(true);
			SandboxToolParameterMenu.instance.DisableParameters();
			SandboxToolParameterMenu.instance.brushRadiusSlider.row.SetActive(true);
			SandboxToolParameterMenuPatch.zoneTypeSelector.row.SetActive(true);
		}

		public override void OnDeactivateTool(InterfaceTool new_tool)
		{
			base.OnDeactivateTool(new_tool);
			SandboxToolParameterMenu.instance.gameObject.SetActive(false);
			SandboxToolParameterMenuPatch.zoneTypeSelector.row.SetActive(false);
		}

		public override void OnMouseMove(Vector3 cursorPos)
		{
			base.OnMouseMove(cursorPos);
		}

		public override void GetOverlayColorData(out HashSet<ToolMenu.CellColorData> colors)
		{
			colors = [];

			foreach (var cell in cellsInRadius)
				colors.Add(new ToolMenu.CellColorData(cell, radiusIndicatorColor));
		}

		public override void OnPaintCell(int cell, int distFromOrigin)
		{
			base.OnPaintCell(cell, distFromOrigin);

			Moonlet_Mod.Instance.AddZoneTypeOverride(cell, targetZoneType);
		}

		public static void DestroyInstance() => Instance = null;
	}
}
