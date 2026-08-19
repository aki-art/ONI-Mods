using UnityEngine;

namespace DecorPackA
{
	// Animations with foreground flagged symbols get a second cached copy in a KAnimLayering instance. 
	// This needs to be also updated here
	public class FGFixer
	{
		public static void FixLayers(GameObject go)
        {
            // thx Sgt
            if (go == null)
                return;


            if (!go.TryGetComponent<KBatchedAnimController>(out var kbac)
                || kbac.layering == null
                || kbac.layering.layerControllers == null
                || !kbac.layering.layerControllers.TryGetValue(KAnim.SymbolFlags.FG, out var fgController))
                return;


            (fgController as KBatchedAnimController)?.SwapAnims(kbac.animFiles);

            // Rehide the symbols from the new foreground animation
            kbac.layering?.HideSymbols();
        }
	}
}
