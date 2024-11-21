using UnityEngine;

namespace DecorPackA
{
	// Animations with foreground flagged symbols get a second cached copy in a KAnimLayering instance. 
	// This needs to be also updated here
	public class FGFixer
	{
		public static void FixLayers(GameObject go)
		{
			if (go == null)
				return;

			var kbac = go.GetComponent<KBatchedAnimController>();

			if (kbac == null || kbac.layering == null) return;

			(kbac.layering.foregroundController as KBatchedAnimController)?.SwapAnims(kbac.animFiles);

			// Rehide the symbols from the new foreground animation
			kbac.layering?.HideSymbols();
		}
	}
}
