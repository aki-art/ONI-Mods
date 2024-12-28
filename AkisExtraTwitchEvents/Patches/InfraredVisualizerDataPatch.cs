using HarmonyLib;
using Twitchery.Content.Scripts;
using UnityEngine;

namespace Twitchery.Patches
{
	public class InfraredVisualizerDataPatch
	{
		[HarmonyPatch(typeof(InfraredVisualizerData), "Update")]
		public class InfraredVisualizerData_Update_Patch
		{
			public static void Postfix(InfraredVisualizerData __instance)
			{
				if (AkisTwitchEvents.Instance.HotTubActive)
					__instance.controller.OverlayColour = (Color)(Color32)SimDebugView.Instance.NormalizedTemperature(9975);
			}
		}
	}
}
