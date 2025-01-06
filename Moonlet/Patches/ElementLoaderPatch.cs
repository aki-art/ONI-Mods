using HarmonyLib;
using System.Collections.Generic;

namespace Moonlet.Patches
{
	public class ElementLoaderPatch
	{
		private static Dictionary<string, SubstanceTable> substanceTablesByDlc;

		[HarmonyPatch(typeof(ElementLoader), nameof(ElementLoader.Load))]
		public class ElementLoader_Load_Patch
		{
			public static void Prefix(Dictionary<string, SubstanceTable> substanceTablesByDlc)
			{
				ElementLoaderPatch.substanceTablesByDlc = substanceTablesByDlc;
			}
		}

		// note: if you are trying to reference this for adding custom elements from code, you do not need this patch
		// elements from the "elements" root folder get automatically picked up by the game
		// this is done this way for the custom extended element data
		[HarmonyPatch(typeof(ElementLoader), nameof(ElementLoader.CollectElementsFromYAML))]
		public class ElementLoader_CollectElementsFromYAML_Patch
		{
			public static void Prefix()
			{
				Mod.elementsLoader.LoadElements(substanceTablesByDlc);
			}

			[HarmonyPriority(Priority.Last)]
			public static void Postfix(ref List<ElementLoader.ElementEntry> __result)
			{
				Mod.elementsLoader.AddElementYamlCollection(ref __result);
			}
		}
	}
}
