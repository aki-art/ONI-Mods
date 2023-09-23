using HarmonyLib;
using UnityEngine;

namespace DecorPackA
{
	// Animations with foreground flagged symbols get a second cached copy in a KAnimLayering instance. 
	// This needs to be also updated here
	public class FGFixer
	{
		private static AccessTools.FieldRef<KBatchedAnimController, KAnimLayering> kAnimLayering;
		private static AccessTools.FieldRef<KAnimLayering, KAnimControllerBase> foregroundController;

		public static void FixLayers(GameObject go)
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
