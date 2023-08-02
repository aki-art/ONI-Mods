using HarmonyLib;
using UnityEngine;

namespace Moonlet.Patches
{
	public class SingleSliderSideScreenPatch
	{
		[HarmonyPatch(typeof(SingleSliderSideScreen), nameof(SingleSliderSideScreen.IsValidForTarget))]
		public class SingleSliderSideScreen_IsValidForTarget_Patch
		{
			public static void Postfix(GameObject target, ref bool __result)
			{
				if (__result)
					__result = !target.HasTag(MTags.ExcludeFromSliderScreen);
			}
		}
	}
}
