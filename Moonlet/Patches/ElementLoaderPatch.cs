using HarmonyLib;
using Moonlet.Elements;
using System.Collections.Generic;

namespace Moonlet.Patches
{
	public class ElementLoaderPatch
	{
		// note: if you are trying to reference this for adding custom elements from code, you do not need this patch
		// elements from the "elements" root folder get automatically picked up by the game
		// this is done this way for the custom extended element data
		public class ElementLoader_CollectElementsFromYAML_Patch
		{
			public static void Patch(Harmony harmony)
			{
				var m_CollectElementsFromYAML = AccessTools.Method(typeof(ElementLoader), nameof(ElementLoader.CollectElementsFromYAML));
				var postfix = AccessTools.Method(typeof(ElementLoader_CollectElementsFromYAML_Patch), nameof(Postfix));

				harmony.Patch(m_CollectElementsFromYAML, postfix: new HarmonyMethod(postfix));

			}
			public static void Postfix(ref List<ElementLoader.ElementEntry> __result)
			{
				foreach (var mod in Mod.modLoaders)
					mod.elementLoader.AddElementYamlCollection(__result);
			}
		}

		public class ElementLoader_Load_Patch
		{
			public static void Patch(Harmony harmony)
			{
				var m_Load = AccessTools.Method(typeof(ElementLoader), nameof(ElementLoader.Load));
				var prefix = AccessTools.Method(typeof(ElementLoader_Load_Patch), nameof(Prefix));
				var postfix = AccessTools.Method(typeof(ElementLoader_Load_Patch), nameof(Postfix));

				harmony.Patch(m_Load, new HarmonyMethod(prefix), new HarmonyMethod(postfix));
			}

			public static void Prefix(Dictionary<string, SubstanceTable> substanceTablesByDlc)
			{
				foreach (var mod in Mod.modLoaders)
				{
					if (mod.elementLoader.HasLoadedElements)
					{
						var list = substanceTablesByDlc[DlcManager.VANILLA_ID].list;
						mod.elementLoader.LoadElements(ref list);
					}
				}
			}

			public static void Postfix()
			{
				ElementUtil.FixTags();
			}
		}
	}
}
