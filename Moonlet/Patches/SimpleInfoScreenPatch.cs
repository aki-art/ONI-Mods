using HarmonyLib;
using Moonlet.Scripts.UI;
using UnityEngine;

namespace Moonlet.Patches
{
	public class SimpleInfoScreenPatch
	{
		[HarmonyPatch(typeof(AdditionalDetailsPanel), nameof(AdditionalDetailsPanel.OnSelectTarget))]
		public class AdditionalDetailsPanel_OnSelectTarget_Patch
		{
			public static void Prefix(ref AdditionalDetailsPanel __instance, GameObject target)
			{
				if (target.TryGetComponent(out KPrefabID _))
				{
					var additionalInfo = __instance.gameObject.AddOrGet<Moonlet_AdditionalInfoScreen>();
					additionalInfo.Initialize(__instance);
					additionalInfo.SetTarget(target);

					additionalInfo.gameObject.SetActive(true);

					return;
				}

				if (__instance.TryGetComponent(out Moonlet_AdditionalInfoScreen screen))
					screen.Hide();
			}
		}
	}
}
