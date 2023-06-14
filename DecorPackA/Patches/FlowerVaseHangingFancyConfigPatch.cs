using DecorPackA.Buildings;
using HarmonyLib;
using UnityEngine;

namespace DecorPackA.Patches
{
	public class FlowerVaseHangingFancyConfigPatch
	{
		[HarmonyPatch(typeof(FlowerVaseHangingFancyConfig), "ConfigureBuildingTemplate")]
		public class FlowerVaseHangingFancyConfig_ConfigureBuildingTemplate_Patch
		{
			private static AccessTools.FieldRef<KBatchedAnimController, KAnimLayering> kAnimLayering;
			private static AccessTools.FieldRef<KAnimLayering, KAnimControllerBase> foregroundController;

			public static void Postfix(GameObject go)
			{
				go.AddOrGet<FacadeRestorer>();
				go.GetComponent<KPrefabID>().prefabSpawnFn += FixLayers;
			}

			// Animations with foreground flagged symbols get a second cached copy in a KAnimLayering instance. 
			// This needs to be also updated here

			private static void FixLayers(GameObject go)
			{
				var kbac = go.GetComponent<KBatchedAnimController>();

				kAnimLayering ??= AccessTools.FieldRefAccess<KBatchedAnimController, KAnimLayering>("layering");
				foregroundController ??= AccessTools.FieldRefAccess<KAnimLayering, KAnimControllerBase>("foregroundController");

				if (kAnimLayering == null || foregroundController == null)
					return;

				var layering = kAnimLayering(kbac);

				if (layering == null)
					return;

				(foregroundController(layering) as KBatchedAnimController).SwapAnims(kbac.animFiles);

				// Rehide the symbols from the new foreground animation
				layering.HideSymbols();
			}
		}
	}
}
