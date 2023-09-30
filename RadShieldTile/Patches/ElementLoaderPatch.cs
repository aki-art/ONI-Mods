using FUtility;
using FUtility.ElementUtil;
using HarmonyLib;
using RadShieldTile.Content;
using System.Collections.Generic;
using UnityEngine;

namespace RadShieldTile.Patches
{
	public class ElementLoaderPatch
	{
		[HarmonyPatch(typeof(ElementLoader), "Load")]
		public class ElementLoader_Load_Patch
		{
			public static void Prefix(Dictionary<string, SubstanceTable> substanceTablesByDlc)
			{
				// Add my new elements
				var list = substanceTablesByDlc[DlcManager.EXPANSION1_ID].GetList();
				RSTElements.RegisterSubstances(list);
			}

			public static void Postfix()
			{
				ElementUtil.FixTags();
			}
		}

		[HarmonyPatch(typeof(ElementLoader), "CollectElementsFromYAML")]
		public class ElementLoader_CollectElementsFromYAML_Patch
		{
			public static void Postfix(ref List<ElementLoader.ElementEntry> __result)
			{
				var radShield = __result.Find(e => e.elementId == RSTElements.RadShield.ToString());

				if (radShield == null)
				{
					Log.Warning($"Could not set properties of {RSTElements.RadShield}");
					return;
				}

				radShield.highTemp = Mod.Settings.MeltingPointKelvin;

				var shielding = Mod.Settings.Shielding / 100f;
				shielding *= 1f / 0.8f;
				shielding = Mathf.Max(0, shielding);

				radShield.radiationAbsorptionFactor = shielding;
			}
		}
	}
}
