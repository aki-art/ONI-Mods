using DecorPackA.Buildings.StainedGlassTile;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace DecorPackA.Patches
{
	internal class BuildToolPatch
	{
		// Change building def places by build tool based on what material is selected
		[HarmonyPatch(typeof(BuildTool), "Activate", typeof(BuildingDef), typeof(IList<Tag>))]
		public static class BuildTool_Activate_Patch
		{
			public static void Prefix(BuildTool __instance, ref BuildingDef def, IList<Tag> selected_elements)
			{
				if (def.PrefabID == DefaultStainedGlassTileConfig.DEFAULT_ID)
				{
					RemoveVisualizer(__instance);

					if (StainedGlassTiles.tileTagDict.TryGetValue(selected_elements[1], out var buildingTag))
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
