using HarmonyLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Patches
{
	public class SimDebugViewPatch
	{
		[HarmonyPatch(typeof(SimDebugView), "GetNormalizedTemperatureColour")]
		public class SimDebugView_GetNormalizedTemperatureColour_Patch
		{
			public static void Postfix(SimDebugView instance, ref Color __result)
			{
				if (AkisTwitchEvents.Instance.HotTubActive)
					__result = instance.NormalizedTemperature(9975);
			}
		}
	}
}
