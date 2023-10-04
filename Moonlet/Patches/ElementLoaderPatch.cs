using HarmonyLib;
using System.Collections.Generic;

namespace Moonlet.Patches
{
	public class ElementLoaderPatch
	{
		[HarmonyPatch(typeof(ElementLoader), "Load")]
		public class ElementLoader_Load_Patch
		{
			public static void Prefix(Dictionary<string, SubstanceTable> substanceTablesByDlc)
			{
				Log.Debug("ElementLoader Load");
				Mod.elementsLoader.LoadElements(substanceTablesByDlc);
			}
		}

		// note: if you are trying to reference this for adding custom elements from code, you do not need this patch
		// elements from the "elements" root folder get automatically picked up by the game
		// this is done this way for the custom extended element data
		[HarmonyPatch(typeof(ElementLoader), "CollectElementsFromYAML")]
		public class ElementLoader_CollectElementsFromYAML_Patch
		{
			public static void Postfix(ref List<ElementLoader.ElementEntry> __result)
			{
				Log.Debug("ElementLoader CollectElementsFromYAML");
				Mod.elementsLoader.AddElementYamlCollection(__result);
			}

			/*			[HarmonyPostfix]
						[HarmonyPriority(Priority.Last)]
						public static void LatePostfix(ref List<ElementLoader.ElementEntry> __result)
						{
							Log.Debuglog("ElementLoader_CollectElementsFromYAML_Patch LatePostfix");
							Mod.sharedElementsLoader.ApplyOverrides(__result);
						}*/
		}
	}
}
