using DecorPackB.Content.Defs.Buildings;
using DecorPackB.Content.Scripts;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackB.Patches
{
	public class BuildToolPatch
	{
		// Change building def places by build tool based on what material is selected
		[HarmonyPatch(typeof(BuildTool), nameof(BuildTool.Activate), typeof(BuildingDef), typeof(IList<Tag>))]
		public static class BuildTool_Activate_Patch
		{
			public static void Prefix(BuildTool __instance, ref BuildingDef def, IList<Tag> selected_elements)
			{
				if (def.PrefabID == DefaultFloorLampConfig.DEFAULT_ID)
				{
					RemoveVisualizer(__instance);

					if (FloorLampFrames.tileTagDict.TryGetValue(selected_elements[0], out var buildingTag))
						def = Assets.GetBuildingDef(buildingTag.ToString());
				}
			}

			// this prevents ghost preview blocks from appearing
			private static void RemoveVisualizer(BuildTool __instance)
			{
				if (__instance.visualizer != null)
				{
					__instance.ClearTilePreview();
					Object.Destroy(__instance.visualizer);
				}
			}
		}
	}
}
